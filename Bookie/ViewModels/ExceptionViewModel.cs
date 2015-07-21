namespace Bookie.ViewModels
{
    using System;
    using Common;

    public class ExceptionViewModel : NotifyBase
    {
        private bool _fatal;
        private string _message;
        private string _moreDetails;

        public string Message
        {
            get
            {
                if (!Fatal) return _message;
                _message += string.Format("{0}{1}{2}", Environment.NewLine, Environment.NewLine,
                    "This is a fatal error. The application will now close.");
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged("Message");
            }
        }

        public string MoreDetails
        {
            get { return _moreDetails; }
            set
            {
                _moreDetails = value;
                NotifyPropertyChanged("MoreDetails");
            }
        }

        public bool Fatal
        {
            get { return _fatal; }
            set
            {
                _fatal = value;
                NotifyPropertyChanged("Fatal");
            }
        }
    }
}