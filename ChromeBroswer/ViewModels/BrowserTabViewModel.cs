using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using BrowserClient.Views;
using Common.Utils;

namespace BrowserClient.ViewModels
{
    public class BrowserTabViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly WebClient _webClient = new WebClient();

        /// <summary>
        ///     网页地址
        /// </summary>
        private string _address;

        private string _favIcon = Path.Combine(ImageHelper.IconPath, ImageHelper.DefaultIconFile);

        /// <summary>
        /// BrowserTab控件
        /// </summary>
        private BrowserTabView _browserTabView;

        /// <summary>
        ///     输出信息
        /// </summary>
        private string _outputMessage;

        /// <summary>
        ///     状态信息
        /// </summary>
        private string _statusMessage;

        /// <summary>
        ///     标题
        /// </summary>
        private string _title;

        /// <summary>
        /// 是否在加载
        /// </summary>
        private bool _isLoading = true;
        
        public BrowserTabViewModel(string address)
        {
            Address = address;
            Title = "正在打开新页面...";
        }

        /// <summary>
        ///     网页地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        /// <summary>
        /// Logo
        /// </summary>
        public string FavIcon
        {
            get
            {
                return _favIcon;
            }
            set
            {
                _favIcon = value;
                OnPropertyChanged("FavIcon");
            }
        }

        /// <summary>
        /// 是否在加载
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value; OnPropertyChanged("IsLoading");
            }
        }

        /// <summary>
        ///     输出信息
        /// </summary>
        public string OutputMessage
        {
            get { return _outputMessage; }
            set
            {
                _outputMessage = value;
                OnPropertyChanged("OutputMessage");
            }
        }

        /// <summary>
        ///     状态信息
        /// </summary>
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged("StatusMessage");
            }
        }

        /// <summary>
        ///     标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        /// <summary>
        ///     当前版本号
        /// </summary>
        public string ClientVersion
        {
            get
            {
                try
                {
                    return Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
                catch
                {
                    return "1.0.0.0";
                }
            }
        }

        /// <summary>
        /// BrowserTab控件
        /// </summary>
        public BrowserTabView BrowserTabView
        {
            get { return _browserTabView; }
            set
            {
                _browserTabView = value;
                OnPropertyChanged("BrowserTabView");
            }
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        public void Dispose()
        {
            IsLoading = false;

            if (null != _browserTabView)
            {
                _browserTabView.TabBrowserControl.Browser.Dispose();
                _browserTabView.TabBrowserControl.Dispose();
                _browserTabView.Dispose();
            }

            if (null != _webClient)
            {
                _webClient.CancelAsync();
                _webClient.Dispose();
            }
        }

        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}