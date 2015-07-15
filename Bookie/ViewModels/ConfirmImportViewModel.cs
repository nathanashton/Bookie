namespace Bookie.ViewModels
{
    using System.Windows.Forms;

    using Bookie.Common;
    using Bookie.Core.Importer;

    public class ConfirmImportViewModel : NotifyBase
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

        public Importer Importer { get; set; }

        public bool SubDirectories
        {
            get
            {
                return _reScrape;
            }
            set
            {
                _reScrape = value;
                NotifyPropertyChanged("SubDirectories");
            }
        }

        public void OK()
        {
            
        }
    }
}