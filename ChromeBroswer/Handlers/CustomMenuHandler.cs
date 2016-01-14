using CefSharp;

namespace BrowserClient.Handlers
{
    internal class CustomMenuHandler : IMenuHandler
    {
        public bool OnBeforeContextMenu(IWebBrowser browser, IContextMenuParams parameters)
        {
            //显示下拉菜单
            return true;
        }
    }
}
