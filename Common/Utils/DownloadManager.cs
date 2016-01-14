using System;
using System.Reflection;
using System.Windows;
using BrowserClient.Views;
using log4net;

namespace BrowserClient.Common.Utils
{
    public class DownloadManager
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void OpenDownloadListView()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    var windows = Application.Current.Windows;
                    DownloadListView downloadListView = null;
                    foreach (var window in windows)
                    {
                        downloadListView = window as DownloadListView;
                        if (downloadListView != null)
                        {
                            break;
                        }
                    }
                    if (null == downloadListView)
                    {
                        downloadListView = new DownloadListView();
                    }
                    downloadListView.Show();
                    downloadListView.Activate();
                }
                catch (Exception exception)
                {
                    _loggor.Error(exception);
                }
            }));
        }

        public void GetRecordList()
        {
            
        }

        public void AddFileRecord()
        {
            
        }
    }
}
