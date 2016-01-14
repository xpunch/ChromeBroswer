using System;
using System.Reflection;
using System.Windows;
using BrowserClient.Common;
using BrowserClient.ViewModels;
using CefSharp;
using CefSharp.Wpf;
using log4net;

namespace BrowserClient.Handlers
{
    internal class CustomLifeSpanHandler : ILifeSpanHandler
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool OnBeforePopup(IWebBrowser browserControl, string sourceUrl, string targetUrl, ref int x, ref int y, ref int width,
            ref int height)
        {
            if (UrlProtocolHelper.IsSupportedProtocol(targetUrl))
            {
                _loggor.InfoFormat("Handler Url Protocol : {0}", targetUrl);
                return UrlProtocolHelper.OnProcess(targetUrl);
            }

            var chromiumWebBrowser = browserControl as ChromiumWebBrowser;
            if (chromiumWebBrowser != null)
            {
                chromiumWebBrowser.Dispatcher.Invoke(new Action(() =>
                {
                    var owner = Window.GetWindow(chromiumWebBrowser);
                    if (owner != null && owner.DataContext is MainViewModel)
                    {
                        var mainViewModel = (MainViewModel)owner.DataContext;
                        mainViewModel.CreateNewTab(targetUrl);
                        _loggor.InfoFormat("Create New Tab : {0}", targetUrl);
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
                chromiumWebBrowser.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        var owner = Window.GetWindow(chromiumWebBrowser);
                        var tabViewModel = chromiumWebBrowser.DataContext as BrowserTabViewModel;

                        if (owner != null && owner.DataContext is MainViewModel && tabViewModel != null)
                        {
                            var mainViewModel = (MainViewModel)owner.DataContext;
                            mainViewModel.CloseTabCommand.Execute(tabViewModel);
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
