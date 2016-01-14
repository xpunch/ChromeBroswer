using System;
using System.Configuration;
using System.Reflection;
using System.Windows;
using CefSharp;
using Common.Const;
using Common.Utils;
using log4net;
using Style.Assets;

namespace BrowserClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// 版本号
        /// </summary>
        public static readonly string VersionNumberString = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
            Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
        
        /// <summary>
        /// 新建页面默认地址
        /// </summary>
        public static string DefaultPageUrl;

        /// <summary>
        /// 门户主页
        /// </summary>
        public static string HomePageUrl;

        public App()
        {
            _loggor.Info("Initialize Settings");
            DispatcherUnhandledException += ApplicationUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            Exit += OnAppExit;

            GetUrls();
            InitSetting();
        }

        private void GetUrls()
        {
            //读取默认主页地址
            try
            {
                var domain = ConfigurationManager.AppSettings["Domain"];
                DefaultPageUrl = domain + ConfigurationManager.AppSettings["DefaultPage"];
                HomePageUrl = domain + ConfigurationManager.AppSettings["HomePage"];
            }
            catch (Exception exception)
            {
                _loggor.Error("Failed to load default url.", exception);
                DefaultPageUrl = BasicUrlValues.EmptyPageUrl;
                HomePageUrl = BasicUrlValues.EmptyPageUrl;
            }
        }

        /// <summary>
        /// 初始化设置
        /// </summary>
        private void InitSetting()
        {
            var settings = new CefSettings
            {
                RemoteDebuggingPort = 8088,
                CachePath = "cache",
                UserAgent = string.Format("ChromeClient/{0} Mozilla/5.0 (Windows NT 6.1; WOW64)  {1} AppleWebKit/537.36 Safari/537.36",
                    Assembly.GetExecutingAssembly().GetName().Version, VersionNumberString),
                IgnoreCertificateErrors = true,
                PackLoadingDisabled = false,
                WindowlessRenderingEnabled = false
            };
            //Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36
            settings.CefCommandLineArgs.Add("debug-plugin-loading", "1");
            settings.CefCommandLineArgs.Add("allow-outdated-plugins", "1");
            settings.CefCommandLineArgs.Add("always-authorize-plugins", "1");
            settings.CefCommandLineArgs.Add("enable-npapi", "1");
            settings.LogSeverity = LogSeverity.Verbose;
            Cef.AddWebPluginDirectory(@".\Plugins");
            Cef.AddWebPluginPath(@".\Plugins\NPSWF32_18_0_0_95.dll");
            
            _loggor.Info("Initialize Cef");
            if (!Cef.Initialize(settings))
            {
                _loggor.Error("Cef initialize failed.");
            }

            if (Cef.IsInitialized)
            {
                _loggor.Info("Cef initialize successed." + VersionNumberString);
            }
            else
            {
                Cef.Shutdown();
                MessageBox.Show(MessageResource.CoreErrorTip);
            }
        }
        
        /// <summary>
        /// 程序域中出现异常，但是没有被捕获
        /// </summary>
        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Cef.Shutdown();

            var exception = e.ExceptionObject as Exception;
            _loggor.Error("CurrentDomainUnhandledException", exception ?? new Exception(e.ToString()));
            MessageBox.Show(MessageResource.AppErrorTip);
        }

        /// <summary>
        /// 程序中出现未捕获的异常
        /// </summary>
        private void ApplicationUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _loggor.Error("ApplicationUnhandledException", e.Exception ?? new Exception(e.ToString()));
            MessageBox.Show(MessageResource.AppErrorTip);
        }

        /// <summary>
        /// 当程序退出时执行，清理临时文件
        /// </summary>
        private void OnAppExit(object sender, ExitEventArgs e)
        {
            Cef.Shutdown();

            DirectoryHelper.DeleteDirectory(ImageHelper.TempIconPath);
        }
    }
}
