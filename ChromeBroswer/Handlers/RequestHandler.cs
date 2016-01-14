using System.Reflection;
using System.Windows;
using CefSharp;
using CefSharp.WinForms;
using log4net;
using Style.Assets;

namespace BrowserClient.Handlers
{
    public class RequestHandler : IRequestHandler
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browserControl, IRequest request, bool isRedirect, bool isMainFrame)
        {
            _loggor.Info("OnBeforeBrowse:"+request.Url);
            var chromiumWebBrowser = browserControl as ChromiumWebBrowser;
            if (chromiumWebBrowser != null)
            {
                chromiumWebBrowser.Title = MessageResource.LoadingTip;
            }
            return false;
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            _loggor.Error(string.Format("OnCertificateError, CefErrorCode:{0}, requestUrl:{1}.", errorCode, requestUrl));
            return true;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            _loggor.Error(string.Format("OnPluginCrashed, PluginPath:{0}.", pluginPath));
            MessageBox.Show("插件加载失败,请稍后重试.");
        }

        bool IRequestHandler.OnBeforeResourceLoad(IWebBrowser browser, IRequest request, bool isMainFrame)
        {
            var headers = request.Headers;
            headers.Add("Accept-Language", "zh,zh-cn");
            request.Headers = headers;
            return false;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            _loggor.Info(string.Format("GetAuthCredentials, Username:{0}, Password:{1}.", username, password));
            return false;
        }

        bool IRequestHandler.OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, WebPluginInfo info)
        {
            _loggor.Info(string.Format("OnBeforePluginLoad, Url:{0}, PolicyUrl:{1}.", url, policyUrl));
            _loggor.Info(string.Format("Plugin Info, Name:{0}, Version:{1}, Path:{2},Description:{3}.", info.Name,
                info.Version, info.Path, info.Description));
            return false;
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {
            _loggor.Error(string.Format("OnRenderProcessTerminated, CefErrorCode:{0}, requestUrl:{1}.", status, browser.Address));
            MessageBox.Show(MessageResource.ProcessTerminatedTip);
        }
    }
}
