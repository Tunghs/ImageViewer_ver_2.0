using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.ViewModel
{
    #region List Member
    public class FileData
    {
        public int FileNo;
        public string FileName;
        public string FilePath;

        public FileData(int no, string filePath)
        {
            FileNo = no;
            FileName = Path.GetFileName(filePath);
            FilePath = filePath;
        }
    }
    #endregion

    public class MainFileListViewModel : ViewModelBase
    {
        #region UIVariable
        public ObservableCollection<FileData> _fileDataCollection = new ObservableCollection<FileData>();
        public ObservableCollection<FileData> FileDataCollection
        {
            get { return _fileDataCollection; }
            set { _fileDataCollection = value; }
        }

        private FileData _sellectedListItem;
        public FileData SellectedListItem
        {
            get { return _sellectedListItem; }
            set { _sellectedListItem = value; RaisePropertyChanged("SellectedItem"); }
        }
        #endregion

        #region Command
        public RelayCommand<EventArgs> SelectedListItemCommand { get; private set; }
        private void InitRelayCommand()
        {
            SelectedListItemCommand = new RelayCommand<EventArgs>(OnSelectedListItem);
        }

        private void OnSelectedListItem(EventArgs e)
        {
            if (SellectedListItem != null)
            {
                if (_ImageChangeEvent != null)
                    _ImageChangeEvent(SellectedListItem.FilePath, false);
            }
        }
        #endregion

        #region Event
        public delegate void ImageChangeHandler(string filePath, bool isAddFile);
        public event ImageChangeHandler _ImageChangeEvent;
        #endregion

        public MainFileListViewModel()
        {
            InitRelayCommand();
            Messenger.Default.Register<NotificationMessage>(this, NotifyMessage);
        }

        private void NotifyMessage(NotificationMessage message)
        {
            if (message.Notification == "AddFileList")
            {
                string tempEvent = message.Sender as string;
                GetFileList(tempEvent);
            }
        }

        private void GetFileList(string filePath)
        {
            FileDataCollection.Clear();
            string fileDirPath = Path.GetDirectoryName(filePath);

            if (Directory.Exists(fileDirPath))
            {

            }
        }
    }
}
