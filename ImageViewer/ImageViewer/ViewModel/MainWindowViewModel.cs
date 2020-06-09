using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack.Dialogs;

using ImageViewer.Controls;

namespace ImageViewer.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region UIVariable
        private string _titleBarText = "Image Viewer";
        public string TitleBarText
        {
            get { return _titleBarText; }
            set { _titleBarText = value; RaisePropertyChanged("TitleBarText"); }
        }

        private bool _isOpenFileList = false;
        public bool IsOpenFileList
        {
            get { return _isOpenFileList; }
            set { _isOpenFileList = value; RaisePropertyChanged("IsOpenFileList"); }
        }

        private bool _isVisibleFileListBtn = true;
        public bool IsVisibleFileListBtn
        {
            get { return _isVisibleFileListBtn; }
            set { _isVisibleFileListBtn = value; RaisePropertyChanged("IsVisibleFileListBtn"); }
        }
        #endregion

        #region Command
        public RelayCommand<object> ButtonClickCommand { get; private set; }
        public RelayCommand<KeyEventArgs> OnKeyDownCommand { get; private set; }

        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
            OnKeyDownCommand = new RelayCommand<KeyEventArgs>(OnKeyDown);
        }

        private void OnButtonClick(object param)
        {
            switch (param.ToString())
            {
                case "OpenFileList":
                    OpenFileList();
                    break;
                case "CloseFileList":
                    CloseFileList();
                    break;
            }
        }

        private void OpenFileList()
        {
            IsOpenFileList = true;
            IsVisibleFileListBtn = false;
        }

        private void CloseFileList()
        {
            IsOpenFileList = false;
            IsVisibleFileListBtn = true;
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            var myMessage = new NotificationMessage(e, "KeyDown");
            Messenger.Default.Send(myMessage);
        }
        #endregion

        #region field
        private List<string> _SetExtensions = new List<string>() { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif", ".tiff" };
        private FileController _FileController = new FileController();
        #endregion

        public DisplayImageViewModel _DisplayImageViewModel { get; set; }
        public MainMenuViewModel _MainMenuViewModel { get; set; }
        public MainFileListViewModel _MainFileListViewModel { get; set; }
        public MainWindowViewModel()
        {
            _DisplayImageViewModel = new DisplayImageViewModel();
            _MainMenuViewModel = new MainMenuViewModel();
            _MainFileListViewModel = new MainFileListViewModel();

            _DisplayImageViewModel._ImageChangeEvent += new DisplayImageViewModel.ImageChangeHandler(this.ReceiveFilePath);
            _MainMenuViewModel._ImageChangeEvent += new MainMenuViewModel.ImageChangeHandler(this.ReceiveFilePath);
            _MainFileListViewModel._ImageChangeEvent += new MainFileListViewModel.ImageChangeHandler(this.ReceiveFilePath);

            InitRelayCommand();
        }

        /// <summary>
        /// 각 도구들에서 경로를 받아 처리
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <param name="isAddFile">참이면 파일리스트 추가</param>
        private void ReceiveFilePath(string filePath, bool isAddFile)
        {
            if (_SetExtensions.Any(e => filePath.ToLower().EndsWith(e)))
            {
                RunImageViewerProcess(filePath, isAddFile);
            }
            else
            {
                MessageBox.Show("지원하지 않는 확장자입니다.", "경고", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RunImageViewerProcess(string filePath, bool isAddFile)
        {
            TitleBarText = filePath;
            _DisplayImageViewModel.DisplayImage = _FileController.GetBitmapImage(filePath);

            if (isAddFile)
            {
                var myMessage = new NotificationMessage(filePath, _SetExtensions, "AddFileList");
                Messenger.Default.Send(myMessage);
            }
        }
    }
}