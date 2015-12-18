using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
using ChromeBrowser.Common;
using ChromeBrowser.ViewModels;

namespace ChromeBrowser.Handlers
{
    public class CustomRequestHandler : IRequestHandler
    {
        public static readonly string VersionNumberString = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
            Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

        public bool OnBeforeBrowse(IWebBrowser browserControl, IRequest request, bool isRedirect, bool isMainFrame)
        {
            return false;
        }

        public bool OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            return false;
        }

        public void OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest request, bool isMainFrame)
        {
            return false;
        }

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm,
            string scheme,
            ref string username, ref string password)
        {
            return false;
        }

        public bool OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, WebPluginInfo info)
        {
            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {
        }
    }
}
