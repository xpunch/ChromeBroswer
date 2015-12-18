using CefSharp;

namespace ChromeBrowser.Handlers
{
    internal class CustomMenuHandler : IMenuHandler
    {
        public bool OnBeforeContextMenu(IWebBrowser browser, IContextMenuParams parameters)
        {
            return true;
        }
    }
}
