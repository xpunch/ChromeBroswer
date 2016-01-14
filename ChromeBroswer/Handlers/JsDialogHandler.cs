using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using BrowserClient.ViewModels;
using BrowserClient.Views;
using CefSharp;
using CefSharp.WinForms;
using log4net;

namespace BrowserClient.Handlers
{
    public class JsDialogHandler : IJsDialogHandler
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool OnJSAlert(IWebBrowser browserControl, string url, string message)
        {
            var notShow = false;

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
                            var mainViewModel = (MainViewModel)mainWindow.DataContext;
                            var selectedTabIndex = -1;
                            for (var i = 0; i < mainViewModel.BrowserTabs.Count; i++)
                            {
                                var browser =
                                    ((BrowserTabUserControl)
                                        mainViewModel.BrowserTabs[i].BrowserTabView.BrowserHost.Child).Browser;
                                if (browserControl.Equals(browser))
                                {
                                    selectedTabIndex = i;
                                }
                            }

                            if (-1 != selectedTabIndex)
                            {
                                mainViewModel.SelectedTabIndex = selectedTabIndex;
                                notShow = false;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        _loggor.Error(exception);
                        notShow = false;
                    }
                }));
            }

            return notShow;
        }

        public bool OnJSConfirm(IWebBrowser browser, string url, string message, out bool retval)
        {
            retval = false;

            return false;
        }

        public bool OnJSPrompt(IWebBrowser browser, string url, string message, string defaultValue, out bool retval, out string result)
        {
            retval = false;
            result = null;

            return false;
        }

        public bool OnJSBeforeUnload(IWebBrowser browser, string message, bool isReload, out bool allowUnload)
        {
            //NOTE: Setting allowUnload to false will cancel the unload request
            allowUnload = false;

            //NOTE: Returning false will trigger the default behaviour, you need to return true to handle yourself.
            return false;
        }
    }
}
