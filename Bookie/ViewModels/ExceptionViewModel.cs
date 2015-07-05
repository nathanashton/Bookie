using System;

namespace Bookie.ViewModels
{
    using Bookie.Common;

    public class ExceptionViewModel : NotifyBase
    {
        private string _message;

        private bool _fatal;

        public string Message
        {
            get
            {
                if (Fatal)
                {
                    _message += String.Format("{0}{1}{2}", Environment.NewLine, Environment.NewLine, "This is a fatal error. The application will now close.");
                    return _message;
                }
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged("Message");
            }
        }

        private string _moreDetails;

        public string MoreDetails
        {
            get
            {
                return _moreDetails;
            }
            set
            {
                _moreDetails = value;
                NotifyPropertyChanged("MoreDetails");
            }
        }

        public bool Fatal
        {
            get
            {
                return _fatal;
            }
            set
            {
                _fatal = value;
                NotifyPropertyChanged("Fatal");
            }
        }
    }
}