using System.Reflection;
using CefSharp;
using log4net;

namespace BrowserClient.Handlers
{
    /// <summary>
    ///     下载
    /// </summary>
    public class CustomDownloadHandler : IDownloadHandler
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool OnBeforeDownload(DownloadItem downloadItem, out string downloadPath, out bool showDialog)
        {
            _loggor.InfoFormat("Url:{0}, FileName:{1}, FullPath:{2}, TotalSize:{3}", downloadItem.Url,
                downloadItem.SuggestedFileName, downloadItem.FullPath, downloadItem.TotalBytes);
            downloadPath = downloadItem.SuggestedFileName;
            showDialog = true;
            return true;
        }

        public bool OnDownloadUpdated(DownloadItem downloadItem)
        {
            if (downloadItem.IsComplete)
            {
                _loggor.InfoFormat("Download Success, FullPath:{0}.", downloadItem.FullPath);
            }
            return false;
        }
    }
}