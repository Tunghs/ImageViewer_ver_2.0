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
    public class MainViewModel : ViewModelBase
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
            }
        }

        private void OpenFileList()
        {
            IsOpenFileList = true;
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            var myMessage = new NotificationMessage(e, "KeyDown");
            Messenger.Default.Send(myMessage);
        }
        #endregion

        #region field
        private List<string> _Ext = new List<string>() { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif", ".tiff" };
        private FileController _FileController = new FileController();
        #endregion

        public DisplayImageViewModel _DisplayImageViewModel { get; set; }
        public MainMenuViewModel _MainMenuViewModel { get; set; }
        public MainFileListViewModel _MainFileListViewModel { get; set; }
        public MainViewModel()
        {
            _DisplayImageViewModel = new DisplayImageViewModel();
            _MainMenuViewModel = new MainMenuViewModel();
            _MainFileListViewModel = new MainFileListViewModel();

            _DisplayImageViewModel._ImageChangeEvent += new DisplayImageViewModel.ImageChangeHandler(this.ReceiveFilePath);

            InitRelayCommand();
        }

        private void ReceiveFilePath(string filePath)
        {
            TitleBarText = filePath;
            _DisplayImageViewModel.DisplayImage = _FileController.GetBitmapImage(filePath);
        }
    }
}