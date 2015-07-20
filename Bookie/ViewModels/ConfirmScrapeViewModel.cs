using Bookie.Common;

namespace Bookie.ViewModels
{
    public class ConfirmScrapeViewModel : NotifyBase
    {
        private bool _generateCovers;

        private bool _reScrape;

        public bool GenerateCovers
        {
            get
            {
                return _generateCovers;
            }
            set
            {
                _generateCovers = value;
                NotifyPropertyChanged("GenerateCovers");
            }
        }

        public bool ReScrape
        {
            get
            {
                return _reScrape;
            }
            set
            {
                _reScrape = value;
                NotifyPropertyChanged("ReScrape");
            }
        }
    }
}