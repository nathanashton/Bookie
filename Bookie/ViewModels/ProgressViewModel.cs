namespace Bookie.ViewModels
{
    using System.Windows;
    using System.Windows.Input;
    using Common;

    public class ProgressViewModel : NotifyBase
    {
        public delegate void CancelDelegate();

        private ICommand _cancelCommand;
        private bool _cancelled;
        private int _downloadProgress;
        private string _operationName;
        private string _progressBarText;
        private int _progressPercentage;
        private string _progressText;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand
                       ?? (_cancelCommand = new RelayCommand(p => CancelOperation(), p => true));
            }
        }

        public bool Cancelled
        {
            get { return _cancelled; }
            set
            {
                _cancelled = value;
                NotifyPropertyChanged("Cancelled");
            }
        }

        public CancelDelegate Cancel { get; set; }
        public Window Window { get; set; }

        public string ProgressBarText
        {
            get { return _progressBarText; }
            set
            {
                _progressBarText = value;
                NotifyPropertyChanged("ProgressBarText");
            }
        }

        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                _progressText = value;
                NotifyPropertyChanged("ProgressText");
            }
        }

        public int ProgressPercentage
        {
            get { return _progressPercentage; }
            set
            {
                _progressPercentage = value;
                NotifyPropertyChanged("ProgressPercentage");
            }
        }

        public string OperationName
        {
            get { return _operationName; }
            set
            {
                _operationName = value;
                NotifyPropertyChanged("OperationName");
            }
        }

        public int DownloadProgress
        {
            get { return _downloadProgress; }
            set
            {
                _downloadProgress = value;
                NotifyPropertyChanged("DownloadProgress");
            }
        }

        public void CancelOperation()
        {
            ProgressService.Cancel();
            //// run deletage
            //if (Cancel == null)
            //{
            //    return;
            //}
            //Cancelled = true;
            //Cancel();
            Window.Close();
        }
    }
}