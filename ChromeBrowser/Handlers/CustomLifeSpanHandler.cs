using System;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using ChromeBrowser.Common;
using ChromeBrowser.ViewModels;

namespace ChromeBrowser.Handlers
{
    internal class CustomLifeSpanHandler : ILifeSpanHandler
    {
        public bool OnBeforePopup(IWebBrowser browserControl, string sourceUrl, string targetUrl, ref int x, ref int y, ref int width,
            ref int height)
        {
            if (UrlProtocolHelper.IsSupportedProtocol(targetUrl))
            {
                return UrlProtocolHelper.OnProcess(targetUrl);
            }

            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            
            chromiumWebBrowser.Dispatcher.Invoke(new Action(() =>
            {
                var owner = Window.GetWindow(chromiumWebBrowser);
                if (owner != null && owner.DataContext is MainViewModel)
                {
                    var mainViewModel = (MainViewModel)owner.DataContext;
                    mainViewModel.CreateNewTab(targetUrl);
                }
            }));

            return true;
        }

        public void OnBeforeClose(IWebBrowser browserControl)
        {
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            chromiumWebBrowser.Dispatcher.Invoke(new Action(() =>
            {
                var owner = Window.GetWindow(chromiumWebBrowser);

                if (owner != null && owner.Content == browserControl)
                {
                    owner.Close();
                }
            }));
        }
    }
}
