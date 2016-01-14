using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using BrowserClient.ViewModels;
using CefSharp;
using log4net;

namespace BrowserClient.Views
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(this);
        }

        /// <summary>
        /// 正在关闭窗体
        /// </summary>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            foreach (var tab in TabBrowserControl.Items)
            {
                var tabViewModel = tab as BrowserTabViewModel;
                if (null != tabViewModel)
                {
                    tabViewModel.Dispose();
                }
            }
        }
    }
}