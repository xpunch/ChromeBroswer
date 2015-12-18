using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp.Wpf;
using ChromeBrowser.Common;
using ChromeBrowser.Views;

namespace ChromeBrowser.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Window _ownerWindow;
        private int _selectedTabIndex;

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged("SelectedTabIndex");
            }
        }
        
        public ObservableCollection<BrowserTabViewModel> BrowserTabs { get; set; }

        public ICommand CloseTabCommand { get; set; }

        public ICommand NewTabCommand { get; set; }

        public MainViewModel(Window ownerWindow)
        {
            _ownerWindow = ownerWindow;
            BrowserTabs = new ObservableCollection<BrowserTabViewModel>();
            OnPropertyChanged("BrowserTabs");

            ownerWindow.Loaded += MainWindowLoaded;
            ownerWindow.Closing+= WindowClosingEvent;
            CloseTabCommand = new RelayCommand<BrowserTabViewModel>(CloseTab);
            NewTabCommand = new RelayCommand(OpenNewTab);
        }

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

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            CreateNewTab(App.DefaultUrl);
        }

        public ICommand JumpCommand { get; set; }

        #region 界面操作

        public void CreateNewTab(string url)
        {
            var newTab = new BrowserTabViewModel(url);
            BrowserTabs.Add(newTab);
            SelectedTabIndex = BrowserTabs.IndexOf(newTab);
        }

        private void OpenNewTab()
        {
            CreateNewTab(App.DefaultUrl);
        }

        private void CloseTab(BrowserTabViewModel closedTab)
        {
            if (BrowserTabs.Count > 0 && BrowserTabs.Contains(closedTab))
            {
                BrowserTabs.Remove(closedTab);
                closedTab.Dispose();
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
