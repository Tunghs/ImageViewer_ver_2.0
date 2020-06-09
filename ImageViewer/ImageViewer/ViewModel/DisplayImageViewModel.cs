using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

using ImageViewer.Controls;

namespace ImageViewer.ViewModel
{
    public class DisplayImageViewModel : ViewModelBase
    {
        #region UI Variable
        private BitmapImage _displayImage;
        public BitmapImage DisplayImage
        {
            get { return _displayImage; }
            set { _displayImage = value; RaisePropertyChanged("DisplayImage"); }
        }

        private Brush _dropAreaColor = (Brush)(new BrushConverter().ConvertFromString("#FF252525"));
        public Brush DropAreaColor
        {
            get { return _dropAreaColor; }
            set { _dropAreaColor = value; RaisePropertyChanged("DropAreaColor"); }
        }

        private bool _isVisibleDropText = true;
        public bool IsVisibleDropText
        {
            get { return _isVisibleDropText; }
            set { _isVisibleDropText = value; RaisePropertyChanged("IsVisibleDropText"); }
        }
        #endregion

        #region Command
        public RelayCommand<DragEventArgs> DropCommand { get; private set; }
        public RelayCommand<DragEventArgs> DragOverCommand { get; private set; }
        public RelayCommand<DragEventArgs> DragLeaveCommand { get; private set; }
        private void InitRelayCommand()
        {
            DropCommand = new RelayCommand<DragEventArgs>(OnImageDrop);
            DragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);
            DragLeaveCommand = new RelayCommand<DragEventArgs>(OnDragLeave);
        }

        #region CommandAction
        private void OnImageDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] dropFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (_ImageChangeEvent != null)
                    _ImageChangeEvent(dropFiles[0]);

                IsVisibleDropText = false;
            }
            DropAreaColor = (System.Windows.Media.Brush)(new BrushConverter().ConvertFromString("#FF252525"));
        }

        private void OnDragOver(DragEventArgs e)
        {
            e.Handled = true;
            DropAreaColor = (System.Windows.Media.Brush)(new BrushConverter().ConvertFromString("#FF2E2E2E"));
        }

        private void OnDragLeave(DragEventArgs e)
        {
            DropAreaColor = (System.Windows.Media.Brush)(new BrushConverter().ConvertFromString("#FF252525"));
        }
        #endregion
        #endregion

        #region Event
        public delegate void ImageChangeHandler(string filePath);
        public event ImageChangeHandler _ImageChangeEvent;
        #endregion

        public DisplayImageViewModel()
        {
            InitRelayCommand();
        }
    }
}
