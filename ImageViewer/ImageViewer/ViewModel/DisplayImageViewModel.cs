using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

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

        private System.Windows.Media.Brush _dropAreaColor = (System.Windows.Media.Brush)(new BrushConverter().ConvertFromString("#FF252525"));
        public System.Windows.Media.Brush DropAreaColor
        {
            get { return _dropAreaColor; }
            set { _dropAreaColor = value; RaisePropertyChanged("DropAreaColor"); }
        }

        private bool _isDropTextVisibility = false;
        public bool IsDropTextVisibility
        {
            get { return _isDropTextVisibility; }
            set { _isDropTextVisibility = value; RaisePropertyChanged("IsDropTextVisibility"); }
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
                //ImageViewerProcess(dropFiles[0], true);
                MessageBox.Show(dropFiles[0]);
                IsDropTextVisibility = false;
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

        public DisplayImageViewModel()
        {
            InitRelayCommand();
        }
    }
}
