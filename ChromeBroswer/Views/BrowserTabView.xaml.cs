using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using BrowserClient.ViewModels;
using CefSharp;
using Common.Const;
using Common.Model;
using Common.Utils;
using log4net;
using Style.Assets;

namespace BrowserClient.Views
{
    /// <summary>
    /// 浏览器Tab内容页面
    /// </summary>
    public partial class BrowserTabView
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ImageHelper _imageHelper = new ImageHelper();

        private readonly UrlHelper _urlHelper = new UrlHelper();

        private string _hostName;

        /// <summary>
        /// 浏览器控件
        /// </summary>
        public BrowserTabUserControl TabBrowserControl;

        public BrowserTabView()
        {
            InitializeComponent();

            DataContextChanged += BrowserTabView_DataContextChanged;
            IsVisibleChanged += BrowserTabView_IsVisibleChanged;
        }

        /// <summary>
        /// DataContext改变的回调函数
        /// </summary>
        private void BrowserTabView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var browserTabViewModel = DataContext as BrowserTabViewModel;
            if (browserTabViewModel != null)
            {
                browserTabViewModel.BrowserTabView = this;
                var address = browserTabViewModel.Address;
                TabBrowserControl = new BrowserTabUserControl(address);
                BrowserHost.Child = TabBrowserControl;
                if (null != TabBrowserControl.Browser)
                {
                    TabBrowserControl.Browser.NavStateChanged += OnBrowserNavStateChanged;
                    TabBrowserControl.Browser.TitleChanged += OnBrowserTitleChanged;
                    TabBrowserControl.Browser.AddressChanged += OnBrowserAddressChanged;
                    TabBrowserControl.Browser.StatusMessage += OnBrowserStatusMessage;
                    TabBrowserControl.Browser.LoadError += OnLoadError;
                    TabBrowserControl.Browser.FrameLoadStart += Browser_FrameLoadStart;
                    TabBrowserControl.Browser.FrameLoadEnd += Browser_FrameLoadEnd;
                }
            }
        }

        private void BrowserTabView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate
                {
                    if (null != TabBrowserControl && null != TabBrowserControl.Browser)
                    {
                        TabBrowserControl.Browser.Focus();
                    }
                }));
            }
        }
        
        #region 响应浏览器事件

        /// <summary>
        ///     跳转后的事件
        /// </summary>
        private void OnBrowserNavStateChanged(object sender, NavStateChangedEventArgs args)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                BackButton.IsEnabled = args.CanGoBack;
                ForwardButton.IsEnabled = args.CanGoForward;
                RefreshButton.IsEnabled = args.CanReload;
                var browserTabViewModel = DataContext as BrowserTabViewModel;
                if (browserTabViewModel != null)
                {
                    browserTabViewModel.IsLoading = args.IsLoading;
                }
            }));
        }

        /// <summary>
        ///     标题修改的回调事件
        /// </summary>
        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var browserTabViewModel = DataContext as BrowserTabViewModel;
                if (browserTabViewModel != null)
                {
                    browserTabViewModel.Title = args.Title;
                }
            }));
        }

        /// <summary>
        ///     网页地址修改的回调函数
        /// </summary>
        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var browserTabViewModel = DataContext as BrowserTabViewModel;
                if (browserTabViewModel != null)
                {
                    try
                    {
                        var hostName = new Uri(_urlHelper.GetHostName(args.Address));
                        _hostName = hostName.Authority;
                        var iconPath = Path.Combine(ImageHelper.TempIconPath, _hostName);
                        if (File.Exists(iconPath))
                        {
                            browserTabViewModel.FavIcon = iconPath;
                        }
                    }
                    catch (Exception exception)
                    {
                        _loggor.Error(exception);
                    }

                    browserTabViewModel.Address = args.Address;
                }
            }));
        }
        
        /// <summary>
        /// 加载过程中出现错误
        /// </summary>
        private void OnLoadError(object sender, LoadErrorEventArgs e)
        {
            if (e.ErrorCode == CefErrorCode.Aborted)
            {
                return;
            }

            _loggor.Error(string.Format("Failed to load URL {0} with {1} error({2}).",
                e.FailedUrl, e.ErrorText, e.ErrorCode));
            if (null != TabBrowserControl && null != TabBrowserControl.Browser)
            {
                using (var fileReader = new StreamReader(new FileStream(@".\Assets\Htmls\Error.html",
                    FileMode.Open), Encoding.UTF8))
                {
                    var errorMessage =
                        string.Format(fileReader.ReadToEnd(),
                            e.FailedUrl, e.ErrorText, e.ErrorCode);
                    TabBrowserControl.Browser.LoadHtml(errorMessage, e.FailedUrl);
                }
            }
        }

        /// <summary>
        /// 状态改变事件
        /// </summary>
        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var browserTabViewModel = DataContext as BrowserTabViewModel;
                if (browserTabViewModel != null)
                {
                    browserTabViewModel.StatusMessage = e.Value;
                }
            }));
        }

        /// <summary>
        ///     浏览器页面加载开始时
        /// </summary>
        private void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var browserTabViewModel = DataContext as BrowserTabViewModel;
                if (browserTabViewModel != null)
                {
                    browserTabViewModel.Title = MessageResource.LoadingTip;
                    browserTabViewModel.IsLoading = true;
                }
                BackButton.IsEnabled = true;
            }));
            try
            {
                if (BasicUrlValues.EmptyPageUrl.Equals(e.Url)) return;
                //Get Host:Port address
                var hostName = new Uri(_urlHelper.GetHostName(e.Url));
                _hostName = hostName.Authority;
                var iconPath = Path.Combine(ImageHelper.TempIconPath, _hostName);
                if (!File.Exists(iconPath))
                {
                    DelegateHelper.RunAsyncAction(new ActionOperModel(),
                        oper =>
                        {
                            oper.Mark = _imageHelper.DownloadImage(hostName + ImageHelper.FavIconFile, iconPath);
                        },
                        DownloadFileCompleted);
                }
            }
            catch (Exception exception)
            {
                _loggor.Error(exception);
            }
        }

        /// <summary>
        ///     加载成功，重新加载logo
        /// </summary>
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    var browserTabViewModel = DataContext as BrowserTabViewModel;
                    if (browserTabViewModel != null)
                    {
                        browserTabViewModel.IsLoading = false;
                        var hostName = new Uri(_urlHelper.GetHostName(browserTabViewModel.Address));
                        _hostName = hostName.Authority;
                        var iconPath = Path.Combine(ImageHelper.TempIconPath, _hostName);
                        if (File.Exists(iconPath))
                        {
                            browserTabViewModel.FavIcon = iconPath;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _loggor.Error(exception);
                }
            }));
        }

        /// <summary>
        ///     下载失败，则拷贝一份logo
        /// </summary>
        private void DownloadFileCompleted(ActionOperModel oper)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (!oper.Mark)
                {
                    try
                    {
                        File.Delete(Path.Combine(ImageHelper.TempIconPath, _hostName));
                        File.Copy(Path.Combine(ImageHelper.IconPath, ImageHelper.DefaultIconFile),
                            Path.Combine(ImageHelper.TempIconPath, _hostName));
                    }
                    catch (Exception exception)
                    {
                        _loggor.Error(exception);
                    }
                }
            }));
        }

        #endregion

        #region 界面操作事件

        private void ForwardCommand(object sender, RoutedEventArgs e)
        {
            if (null != TabBrowserControl && null != TabBrowserControl.Browser && TabBrowserControl.Browser.CanGoForward)
            {
                TabBrowserControl.Browser.Forward();
                BackButton.IsEnabled = true;
                TabBrowserControl.Browser.Focus();
            }
        }

        private void BackCommand(object sender, RoutedEventArgs e)
        {
            if (null != TabBrowserControl && null != TabBrowserControl.Browser && TabBrowserControl.Browser.CanGoBack)
            {
                TabBrowserControl.Browser.Back();
                TabBrowserControl.Browser.Focus();
            }
        }

        /// <summary>
        ///     Enter按下
        /// </summary>
        private void EnterEeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (null != TabBrowserControl && null != TabBrowserControl.Browser)
                {
                    var browserTabViewModel = DataContext as BrowserTabViewModel;
                    if (browserTabViewModel != null)
                    {
                        browserTabViewModel.IsLoading = true;
                        var jumpUrl = browserTabViewModel.Address;
                        TabBrowserControl.Browser.Load(string.IsNullOrWhiteSpace(jumpUrl)
                            ? BasicUrlValues.EmptyPageUrl
                            : jumpUrl);
                        BackButton.IsEnabled = true;
                        TabBrowserControl.Browser.Focus();
                    }
                }
            }
        }

        private void RefreshCommand(object sender, RoutedEventArgs e)
        {
            if (null != TabBrowserControl && null != TabBrowserControl.Browser && TabBrowserControl.Browser.CanReload)
            {
                TabBrowserControl.Browser.Reload(true);
                BackButton.IsEnabled = true;
                TabBrowserControl.Browser.Focus();
            }
        }

        private void SettingCommand(object sender, RoutedEventArgs e)
        {
            SettingMenu.IsOpen = !SettingMenu.IsOpen;
        }

        private void ClearCacheCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                var cacheFolder = Path.Combine(Environment.CurrentDirectory, "cache");
                var files = Directory.GetFiles(cacheFolder);
                foreach (var file in files)
                {
                    try
                    {
                        var fileName = Path.GetFileName(file);
                        if (!string.IsNullOrEmpty(fileName) && (fileName.StartsWith("data_")||fileName.StartsWith("f_")))
                        {
                            File.Delete(file); 
                        }
                    }
                    catch
                    {
                        //unable to delete
                    }
                }
            }
            catch (Exception exception)
            {
                _loggor.Error(exception);
            }
        }

        private void OpenNewTabCommand(object sender, RoutedEventArgs e)
        {
            OpenNewTab(App.DefaultPageUrl);
        }

        private void OpenNewTab(string url)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null && mainWindow.DataContext is MainViewModel)
                {
                    var mainViewModel = (MainViewModel)mainWindow.DataContext;
                    mainViewModel.CreateNewTab(url);
                    _loggor.InfoFormat("Jump User Center : {0}", url);
                }
            }));
        }

        private void ExitCommand(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        #endregion

        /// <summary>
        ///     Dispose
        /// </summary>
        public void Dispose()
        {
            if (null != TabBrowserControl)
            {
                TabBrowserControl.Browser.Dispose();
                TabBrowserControl.Dispose();
            }
        }
    }
}