using System;
using System.Reflection;
using System.Windows;
using BrowserClient.ViewModels;
using CefSharp;
using CefSharp.WinForms;
using Common.Utils;
using log4net;

namespace BrowserClient.Handlers
{
    public class LifeSpanHandler : ILifeSpanHandler
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool OnBeforePopup(IWebBrowser browserControl, string sourceUrl, string targetUrl, ref int x, ref int y,
            ref int width, ref int height)
        {
            if (UrlProtocolHelper.IsSupportedProtocol(targetUrl))
            {
                _loggor.InfoFormat("Handler Url Protocol : {0}", targetUrl);
                return UrlProtocolHelper.OnProcess(targetUrl);
            }

            var chromiumWebBrowser = browserControl as ChromiumWebBrowser;
            if (chromiumWebBrowser != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    var mainWindow = Application.Current.MainWindow;
                    if (mainWindow != null && mainWindow.DataContext is MainViewModel)
                    {
                        var mainViewModel = (MainViewModel) mainWindow.DataContext;
                        mainViewModel.CreateNewTab(targetUrl);
                        _loggor.InfoFormat("Create New Tab : {0}", targetUrl);
                    }
                }));
            }

            return true;
        }

        public bool DoClose(IWebBrowser browserControl)
        {
            _loggor.Info("DoClose");
            var chromiumWebBrowser = browserControl as ChromiumWebBrowser;
            if (chromiumWebBrowser != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        var mainWindow = Application.Current.MainWindow;
                        if (mainWindow != null && mainWindow.DataContext is MainViewModel)
                        {
                            var mainViewModel = (MainViewModel) mainWindow.DataContext;
                            if (null != mainViewModel.BrowserTabs && -1 != mainViewModel.SelectedTabIndex &&
                                mainViewModel.BrowserTabs.Count >= mainViewModel.SelectedTabIndex)
                            {
                                var selectedTab = mainViewModel.BrowserTabs[mainViewModel.SelectedTabIndex];
                                if (null != selectedTab)
                                {
                                    mainViewModel.CloseTabCommand.Execute(selectedTab);
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        _loggor.Error("Failed to close tab.", exception);
                    }
                }));
            }
            return true;
        }

        public void OnBeforeClose(IWebBrowser browserControl)
        {
            _loggor.Info("OnBeforeClose");
            var chromiumWebBrowser = browserControl as ChromiumWebBrowser;
            if (chromiumWebBrowser != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        var mainWindow = Application.Current.MainWindow;
                        if (mainWindow != null && mainWindow.DataContext is MainViewModel)
                        {
                            var mainViewModel = (MainViewModel) mainWindow.DataContext;
                            if (null != mainViewModel.BrowserTabs && -1 != mainViewModel.SelectedTabIndex &&
                                mainViewModel.BrowserTabs.Count >= mainViewModel.SelectedTabIndex)
                            {
                                var selectedTab = mainViewModel.BrowserTabs[mainViewModel.SelectedTabIndex];
                                if (null != selectedTab)
                                {
                                    mainViewModel.CloseTabCommand.Execute(selectedTab);
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        _loggor.Error("Failed to close tab.", exception);
                    }
                }));
            }
        }
    }
}