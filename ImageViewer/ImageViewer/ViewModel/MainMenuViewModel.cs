using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageViewer.ViewModel
{
    public class MainMenuViewModel : ViewModelBase
    {
        #region Command
        public RelayCommand<object> ButtonClickCommand { get; private set; }
        private void InitRelayCommand()
        {
            ButtonClickCommand = new RelayCommand<object>(OnButtonClick);
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
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = _InitialDialogPath;
                dialog.Filters.Add(new CommonFileDialogFilter("All files", "*.*"));

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    _InitialDialogPath = Path.GetDirectoryName(dialog.FileName);

                    if (_ImageChangeEvent != null)
                        _ImageChangeEvent(dialog.FileName, true);
                }
            }
        }

        public void ExitProgram()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        #endregion

        #region field
        private string _InitialDialogPath = @"C:\\";
        #endregion

        #region Event
        public delegate void ImageChangeHandler(string filePath, bool isAddFile);
        public event ImageChangeHandler _ImageChangeEvent;
        #endregion

        public MainMenuViewModel()
        {
            InitRelayCommand();
        }
    }
}
