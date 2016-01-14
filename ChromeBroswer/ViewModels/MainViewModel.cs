using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Common;
using Common.Model;

namespace BrowserClient.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int _selectedTabIndex;

        public MainViewModel(Window ownerWindow)
        {
            BrowserTabs = new ObservableCollection<BrowserTabViewModel>();
            OnPropertyChanged("BrowserTabs");

            ownerWindow.Loaded += MainWindowLoaded;
            ownerWindow.Closing+= WindowClosingEvent;
            CloseTabCommand = new RelayCommand<BrowserTabViewModel>(CloseTab);
            NewTabCommand = new RelayCommand(OpenNewTab);
        }

        /// <summary>
        /// 当前选中的页面索引
        /// </summary>
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged("SelectedTabIndex");
            }
        }

        /// <summary>
        /// 所有网页的集合
        /// </summary>
        public ObservableCollection<BrowserTabViewModel> BrowserTabs { get; set; }

        /// <summary>
        /// 新页面
        /// </summary>
        public ICommand NewTabCommand { get; set; }

        /// <summary>
        /// 关闭当前页面
        /// </summary>
        public ICommand CloseTabCommand { get; set; }

        #region 界面操作

        /// <summary>
        /// 创建一个新页面
        /// </summary>
        public void CreateNewTab(string url)
        {
            var newTab = new BrowserTabViewModel(url);
            BrowserTabs.Add(newTab);
            SelectedTabIndex = BrowserTabs.IndexOf(newTab);
            OnPropertyChanged("BrowserTabs");
        }

        /// <summary>
        /// 打开一个新页面
        /// </summary>
        private void OpenNewTab()
        {
            CreateNewTab(App.DefaultPageUrl);
        }

        /// <summary>
        /// 关闭当前页面
        /// </summary>
        private void CloseTab(BrowserTabViewModel closedTab)
        {
            if (BrowserTabs.Count > 0 && BrowserTabs.Contains(closedTab))
            {
                BrowserTabs.Remove(closedTab);
                closedTab.Dispose();
            }

            if (BrowserTabs.Count <= 0)
            {
                OpenNewTab();
            }
        }

        /// <summary>
        /// 关闭窗体时，释放所有页面
        /// </summary>
        private void WindowClosingEvent(object sender, CancelEventArgs e)
        {
            if (null != BrowserTabs && BrowserTabs.Any())
            {
                foreach (var tabViewModel in BrowserTabs)
                {
                    if (null != tabViewModel)
                    {
                        tabViewModel.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 加载窗体时，默认打开登录页面
        /// </summary>
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            CreateNewTab(App.DefaultPageUrl);
        }

        #endregion
    }
}
