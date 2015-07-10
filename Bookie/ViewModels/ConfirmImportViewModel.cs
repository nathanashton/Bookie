using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookie.ViewModels
{
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
