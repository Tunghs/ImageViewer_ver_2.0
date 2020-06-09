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
        public int FileNo { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

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
                string filePath = message.Sender as string;
                List<string> setExtensions = message.Target as List<string>;
                GetFileList(filePath, setExtensions);
            }
        }

        private void GetFileList(string filePath, List<string> setExtensions)
        {
            FileDataCollection.Clear();
            string fileDirPath = Path.GetDirectoryName(filePath);

            if (Directory.Exists(fileDirPath))
            {
                var imageFilePaths = Directory.GetFiles(fileDirPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => setExtensions.Any(e => s.ToLower().EndsWith(e)));

                int imageCount = 1;
                foreach (string imageFilePath in imageFilePaths)
                {
                    FileData fileData = new FileData(imageCount, imageFilePath);
                    FileDataCollection.Add(fileData);
                    imageCount++;
                }
            }
        }
    }
}
