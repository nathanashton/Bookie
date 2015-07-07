namespace Bookie.ViewModels
{
    using Bookie.Common;
    using Bookie.Common.Model;
    using Bookie.Core.Domains;
    using Bookie.Core.Importer;
    using Bookie.Core.Scraper;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;

    public class SourceDirectoryViewModel : NotifyBase
    {
        private readonly SourceDirectoryDomain _domain;
        private ObservableCollection<SourceDirectory> _sourceDirectories;
        private SourceDirectory _selectedSourceDirectory;

        private Importer _importer;
        public MainViewModel MainViewModel;

        private bool _progressReportingActive;

        public bool ProgressReportingActive
        {
            get
            {
                return _progressReportingActive;
            }
            set
            {
                _progressReportingActive = value;
                NotifyPropertyChanged("ProgressReportingActive");
            }
        }

        private ICommand _removeCommand;

        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand
                       ?? (_removeCommand = new RelayCommand(p => Remove(), p => SelectedSourceDirectory != null));
            }
        }

        private ICommand scanCommand;

        public ICommand ScanCommand
        {
            get
            {
                return scanCommand
                       ?? (scanCommand = new RelayCommand(p => Scan(), p => SelectedSourceDirectory != null));
            }
        }

        private ICommand _addCommand;

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand(p => Add(), p => _domain != null));
            }
        }

        private ICommand _scrapeCOmmand;

        public ICommand ScrapeCommand
        {
            get
            {
                return _scrapeCOmmand
                       ?? (_scrapeCOmmand = new RelayCommand(p => Scrape(), p => SelectedSourceDirectory != null && SelectedSourceDirectory.Books.Count > 0));
            }
        }

        public SourceDirectory SelectedSourceDirectory
        {
            get
            {
                return _selectedSourceDirectory;
            }
            set
            {
                _selectedSourceDirectory = value;
                NotifyPropertyChanged("SelectedSourceDirectory");
            }
        }


        public ObservableCollection<SourceDirectory> SourceDirectories
        {
            get
            {
                return _sourceDirectories;
            }
            set
            {
                _sourceDirectories = value;
                NotifyPropertyChanged("SourceDirectories");
            }
        }

        public SourceDirectoryViewModel(MainViewModel v)
        {
            MainViewModel = v;
            _domain = new SourceDirectoryDomain();
            SourceDirectories = new ObservableCollection<SourceDirectory>();

            Refresh();
        }

        public async void Refresh()
        {
            SourceDirectories.Clear();
            var allSources = await _domain.GetAllSourceDirectoriesAsync();
            SourceDirectories = new ObservableCollection<SourceDirectory>(allSources);
            MainViewModel.RefreshAllBooks();
            MainViewModel.RefreshPublishersAndAuthors();
        }

        public void Add()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var source = new SourceDirectory { SourceDirectoryUrl = dialog.SelectedPath };

            if (_domain.Exists(source.SourceDirectoryUrl))
            {
                // MessageBox.Show(Resources.Message_SourceDirectoryExists, Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            source.EntityState = EntityState.Added;
            _domain.AddSourceDirectory(source);

            Refresh();

            var result = MessageBox.Show(
            "Do you want to scan?", "c",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            SelectedSourceDirectory = source;
            Scan();
        }

        public void Remove()
        {
            SelectedSourceDirectory.EntityState = EntityState.Deleted;
             _domain.RemoveSourceDirectory(SelectedSourceDirectory);
            Refresh();
        }

        public void Scan()
        {
            SelectedSourceDirectory.DateLastImported = DateTime.Now;
            SelectedSourceDirectory.EntityState = EntityState.Modified;
            _domain.UpdateSourceDirectory(SelectedSourceDirectory);

            ProgressReportingActive = true;

            _importer = new Importer(SelectedSourceDirectory);
            _importer.BookChanged += MainViewModel.i_BookChanged;
            _importer.Worker.RunWorkerCompleted += _worker_RunWorkerCompleted;

            _importer.ScanSource(false, false);
            _importer.ProgressComplete += delegate { ProgressReportingActive = false; };

            Refresh();
        }

        private void _worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Refresh();
        }

        public void Scrape()
        {
            var scraper = new Scraper();

            ProgressReportingActive = true;

            scraper.BookChanged += MainViewModel.i_BookChanged;
            scraper.Worker.RunWorkerCompleted += _worker_RunWorkerCompleted;

            scraper.Scrape(SelectedSourceDirectory, MainViewModel.Books.Cast<Book>().ToList());
            scraper.ProgressComplete += delegate { ProgressReportingActive = false; };
            Refresh();
        }
    }
}