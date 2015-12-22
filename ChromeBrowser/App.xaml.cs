using System;
using System.Reflection;
using CefSharp;

namespace ChromeBrowser
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public const string DefaultUrl = "http://ip.chinaz.com/";

        public App()
        {
            var settings = new CefSettings();
            settings.RemoteDebuggingPort = 8088;
            //The location where cache data will be stored on disk. If empty an in-memory cache will be used for some features and a temporary disk cache for others.
            //HTML5 databases such as localStorage will only persist across sessions if a cache path is specified. 
            settings.CachePath = "cache";
            //settings.UserAgent = "CefSharp Browser" + Cef.CefSharpVersion; // Example User Agent
            //settings.CefCommandLineArgs.Add("renderer-process-limit", "1");
            //settings.CefCommandLineArgs.Add("renderer-startup-dialog", "1");
            //settings.CefCommandLineArgs.Add("enable-media-stream", "1"); //Enable WebRTC
            //settings.CefCommandLineArgs.Add("no-proxy-server", "1"); //Don't use a proxy server, always make direct connections. Overrides any other proxy server flags that are passed.
            //settings.CefCommandLineArgs.Add("debug-plugin-loading", "1"); //Dumps extra logging about plugin loading to the log file.
            //settings.CefCommandLineArgs.Add("disable-plugins-discovery", "1"); //Disable discovering third-party plugins. Effectively loading only ones shipped with the browser plus third-party ones as specified by --extra-plugin-dir and --load-plugin switches
            //settings.CefCommandLineArgs.Add("enable-system-flash", "1"); //Automatically discovered and load a system-wide installation of Pepper Flash.
            settings.CefCommandLineArgs.Add("debug-plugin-loading", "1");
            settings.CefCommandLineArgs.Add("allow-outdated-plugins", "1");
            settings.CefCommandLineArgs.Add("always-authorize-plugins", "1");
            try
            {
                var clientVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var cefVersion = string.Format("Chrome/{0}, CEF/{1}, CefSharp/{2}", Cef.ChromiumVersion,
                    Cef.CefVersion, Cef.CefSharpVersion);
                settings.UserAgent = string.Format("BrowserClient/{0}  {1}", clientVersion, cefVersion);
                settings.ProductVersion = clientVersion;
                //settings.WindowlessRenderingEnabled = true;
            }
            catch (Exception exception)
            {
                //do nothing
            }
            //settings.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Client/39.0.2171.95";
            //Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240 
            //settings.CefCommandLineArgs.Add("enable-npapi", "1");
            //settings.CefCommandLineArgs.Add("ppapi-flash-path", @".\PepperFlash\pepflashplayer32_20_0_0_248.dll"); //Load a specific pepper flash version (Step 1 of 2)
            //settings.CefCommandLineArgs.Add("ppapi-flash-version", "19.0.0.185"); //Load a specific pepper flash version (Step 2 of 2)

            //NOTE: For OSR best performance you should run with GPU disabled:
            // `--disable-gpu --disable-gpu-compositing --enable-begin-frame-scheduling`
            // (you'll loose WebGL support but gain increased FPS and reduced CPU usage).
            // http://magpcss.org/ceforum/viewtopic.php?f=6&t=13271#p27075
            //https://bitbucket.org/chromiumembedded/cef/commits/e3c1d8632eb43c1c2793d71639f3f5695696a5e8

            //NOTE: The following function will set all three params
            //settings.SetOffScreenRenderingBestPerformanceArgs();
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");

            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1"); //Disable Vsync

            //Disables the DirectWrite font rendering system on windows.
            //Possibly useful when experiencing blury fonts.
            //settings.CefCommandLineArgs.Add("disable-direct-write", "1");

            //settings.MultiThreadedMessageLoop = multiThreadedMessageLoop;

            /*var osr = true;
            // Off Screen rendering (WPF/Offscreen)
            if (osr)
            {
                settings.WindowlessRenderingEnabled = true;
                // Disable Surfaces so internal PDF viewer works for OSR
                // https://bitbucket.org/chromiumembedded/cef/issues/1689
                //settings.CefCommandLineArgs.Add("disable-surfaces", "1");
                //settings.EnableInternalPdfViewerOffScreen();
                settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");
            }*/

            /*var proxy = ProxyConfig.GetProxyInformation();
            switch (proxy.AccessType)
            {
                case InternetOpenType.Direct:
                    {
                        //Don't use a proxy server, always make direct connections.
                        settings.CefCommandLineArgs.Add("no-proxy-server", "1");
                        break;
                    }
                case InternetOpenType.Proxy:
                    {
                        settings.CefCommandLineArgs.Add("proxy-server", proxy.ProxyAddress);
                        break;
                    }
                case InternetOpenType.PreConfig:
                    {
                        settings.CefCommandLineArgs.Add("proxy-auto-detect", "1");
                        break;
                    }
            }*/

            settings.LogSeverity = LogSeverity.Verbose;

            /*if (DebuggingSubProcess)
            {
                var architecture = Environment.Is64BitProcess ? "x64" : "x86";
                settings.BrowserSubprocessPath = "..\\..\\..\\..\\CefSharp.BrowserSubprocess\\bin\\" + architecture + "\\Debug\\CefSharp.BrowserSubprocess.exe";
            }*/

            /*settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeNameTest,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });

            settings.RegisterExtension(new CefExtension("cefsharp/example", Resources.extension));

            settings.EnableFocusedNodeChanged = true;

            Cef.OnContextInitialized = delegate
            {
                var cookieManager = Cef.GetGlobalCookieManager();
                cookieManager.SetStoragePath("cookies", true);
                cookieManager.SetSupportedSchemes("custom");
            };*/
            //Cef.AddWebPluginDirectory(@".\PepperFlash");
            //Cef.AddWebPluginPath(@".\PepperFlash\pepflashplayer32_20_0_0_248.dll");
            Cef.AddWebPluginDirectory(@".\Plugins");
            Cef.AddWebPluginPath(@".\Plugins\NPSWF32_18_0_0_95.dll");
            Cef.Initialize(settings);
        }
    }
}
