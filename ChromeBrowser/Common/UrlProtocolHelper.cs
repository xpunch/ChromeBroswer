using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace ChromeBrowser.Common
{
    /// <summary>
    /// 调用UrlProtocol
    /// </summary>
    public class UrlProtocolHelper
    {
        public static List<string> SupportedProtocol = new List<string>
        {
            "tencent",
            "evernote"
        };

        public static bool IsSupportedProtocol(string url)
        {
            if (string.IsNullOrEmpty(url) /*|| Regex.IsMatch(url, @"(?<=://).+?(?=:|/|\Z)")*/)
            {
                return false;
            }

            return SupportedProtocol.Any(url.StartsWith);
        }

        /// <summary>
        /// 执行
        /// </summary>
        public static bool OnProcess(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            var protocolKey = url.Split(':').FirstOrDefault();
            if (string.IsNullOrEmpty(protocolKey))
            {
                return false;
            }

            using (var urlRegistry = Registry.ClassesRoot.OpenSubKey(protocolKey, false))
            {
                if (urlRegistry != null)
                {
                    var appPath = urlRegistry.GetValue("URL Protocol").ToString();
                    if (File.Exists(appPath))
                    {
                        var myprocess = new Process();
                        var startInfo = new ProcessStartInfo(appPath, url);
                        myprocess.StartInfo = startInfo;
                        myprocess.StartInfo.UseShellExecute = false;
                        myprocess.Start();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
