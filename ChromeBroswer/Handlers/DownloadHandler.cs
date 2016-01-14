using System;
using System.Reflection;
using System.Windows;
using CefSharp;
using log4net;

namespace BrowserClient.Handlers
{
    public class DownloadHandler : IDownloadHandler
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool OnBeforeDownload(DownloadItem downloadItem, out string downloadPath, out bool showDialog)
        {
            _loggor.InfoFormat("Id:{4},Url:{0}, FileName:{1}, FullPath:{2}, TotalSize:{3}", downloadItem.Url,
                downloadItem.SuggestedFileName, downloadItem.FullPath, downloadItem.TotalBytes, downloadItem.Id);
            downloadPath = downloadItem.SuggestedFileName;
            showDialog = true;

            return true;
        }

        public bool OnDownloadUpdated(DownloadItem downloadItem)
        {
            if (downloadItem.IsCancelled)
            {
                /*Application.Current.MainWindow.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show("已取消下载.", "下载", MessageBoxButton.OK, MessageBoxImage.Information);
                }));*/
                _loggor.InfoFormat("Download Cancelled, Url:{0}.", downloadItem.Url);
            }

            if (downloadItem.IsComplete)
            {
                var message = string.Format("文件下载成功.保存路径:{0}", downloadItem.FullPath);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(message, "下载", MessageBoxButton.OK, MessageBoxImage.Information);
                }));
                _loggor.InfoFormat("Download Success, FullPath:{0}.", downloadItem.FullPath);
            }

            return false;
        }
    }
}
