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
                case "OpenFileBrowser":
                    OpenFileBrowser();
                    break;
                case "ExitProgram":
                    ExitProgram();
                    break;
            }
        }

        private void OpenFileBrowser()
        {
            using(CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = _InitialDialogPath;
                dialog.Filters.Add(new CommonFileDialogFilter("All files", "*.*"));

                if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    _InitialDialogPath = Path.GetDirectoryName(dialog.FileName);
                    MessageBox.Show(dialog.FileName);
                }
            }
        }

        public void ExitProgram()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            var myMessage = new NotificationMessage(e, "KeyDown");
            Messenger.Default.Send(myMessage);
        }
        #endregion

        #region field
        private string _FilePath = "";
        private string _InitialDialogPath = @"C:\\";
        private List<string> _Ext = new List<string>() { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tif", ".tiff" };
        private FileController _FileController = new FileController();
        #endregion

        public DisplayImageViewModel _DisplayImageViewModel { get; set; }
        public MainMenuViewModel _MainMenuViewModel { get; set; }
        public MainViewModel()
        {
            _DisplayImageViewModel = new DisplayImageViewModel();
            _MainMenuViewModel = new MainMenuViewModel();

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