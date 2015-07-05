namespace Bookie.ViewModels
{
    using Bookie.Common;
    using System.Windows;

    public class ProgressViewModel : NotifyBase
    {
        private string _progressText;

        private int _progressPercentage;

        private string _operationName;

        private int _downloadProgress;

        private string _progressBarText;

        private bool _cancelled;

        public bool Cancelled
        {
            get
            {
                return _cancelled;
            }
            set
            {
                _cancelled = value;
                NotifyPropertyChanged("Cancelled");
            }
        }

        public delegate void CancelDelegate();

        public CancelDelegate Cancel { get; set; }

        public Window Window { get; set; }

        public string ProgressBarText
        {
            get
            {
                return _progressBarText;
            }
            set
            {
                _progressBarText = value;
                NotifyPropertyChanged("ProgressBarText");
            }
        }

        public string ProgressText
        {
            get
            {
                return _progressText;
            }
            set
            {
                _progressText = value;
                NotifyPropertyChanged("ProgressText");
            }
        }

        public int ProgressPercentage
        {
            get
            {
                return _progressPercentage;
            }
            set
            {
                _progressPercentage = value;
                NotifyPropertyChanged("ProgressPercentage");
            }
        }

        public string OperationName
        {
            get
            {
                return _operationName;
            }
            set
            {
                _operationName = value;
                NotifyPropertyChanged("OperationName");
            }
        }

        public int DownloadProgress
        {
            get
            {
                return _downloadProgress;
            }
            set
            {
                _downloadProgress = value;
                NotifyPropertyChanged("DownloadProgress");
            }
        }

        public void CancelOperation()
        {
            // run deletage
            if (Cancel == null)
            {
                return;
            }
            Cancelled = true;
            Cancel();
            Window.Close();
        }
    }
}