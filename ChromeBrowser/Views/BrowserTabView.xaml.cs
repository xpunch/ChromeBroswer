// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Controls;
using System.Windows.Input;
using CefSharp;
using ChromeBrowser.Handlers;

namespace ChromeBrowser.Views
{
    public partial class BrowserTabView
    {
        public BrowserTabView()
        {
            InitializeComponent();


            browser.LifeSpanHandler = new CustomLifeSpanHandler();
            browser.MenuHandler = new CustomMenuHandler();
            //browser.GeolocationHandler = new GeolocationHandler();
            browser.DownloadHandler = new CustomDownloadHandler();
            browser.RequestHandler = new CustomRequestHandler();
            //You can specify a custom RequestContext to share settings amount groups of ChromiumWebBrowsers
            //Also this is now the only way to access OnBeforePluginLoad - need to implement IPluginHandler
            //browser.RequestContext = new RequestContext(new PluginHandler());
            
            //browser.RequestContext.RegisterSchemeHandlerFactory(CefSharpSchemeHandlerFactory.SchemeName, null, new CefSharpSchemeHandlerFactory());
            //browser.RenderProcessMessageHandler = new RenderProcessMessageHandler();
        }
    }
}
