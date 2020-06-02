using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace ImageViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region UIVariable
        private string _title = "ImageViewer";
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged("Title"); }
        }
        #endregion

        #region Command
        public RelayCommand<KeyEventArgs> OnKeyDownCommand { get; private set; }

        private void InitRelayCommand()
        {
            OnKeyDownCommand = new RelayCommand<KeyEventArgs>(OnKeyDown);
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            var myMessage = new NotificationMessage(e, "KeyDown");
            Messenger.Default.Send(myMessage);
        }
        #endregion

        public DisplayImageViewModel _DisplayImageViewModel { get; set; }
        public MainViewModel()
        {
            _DisplayImageViewModel = new DisplayImageViewModel();

            InitRelayCommand();
        }

        private void ReceiveTitle(string text)
        {
            Title = text;
        }
    }
}