using System.Collections.ObjectModel;
using Common.Model;

namespace BrowserClient.ViewModels
{
    public class DownloadListViewModel : BaseViewModel
    {
        private ObservableCollection<DownloadFile> _fileList;

        public ObservableCollection<DownloadFile> FileList
        {
            get { return _fileList; }
            set
            {
                _fileList = value;
                OnPropertyChanged("FileList");
            }
        }
    }
}