using System;
using System.Drawing;
using System.Windows.Forms;
using BrowserClient.Handlers;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;

namespace BrowserClient.Views
{
    public partial class BrowserTabUserControl : UserControl
    {
        public ChromiumWebBrowser Browser { get; private set; }

        public BrowserTabUserControl(string address)
        {
            InitializeComponent();

            var browser = new ChromiumWebBrowser(address)
            {
                Dock = DockStyle.Fill
            };
            browserPanel.Controls.Add(browser);

            Browser = browser;

            browser.MenuHandler = new MenuHandler();
            browser.RequestHandler = new RequestHandler();
            browser.JsDialogHandler = new JsDialogHandler();
            browser.GeolocationHandler = new GeolocationHandler();
            browser.DownloadHandler = new DownloadHandler();
            browser.LifeSpanHandler = new LifeSpanHandler();
            browser.DragHandler = new DragHandler();
            browser.ContextMenu = new ContextMenu();
        }
    }
}