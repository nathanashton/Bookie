namespace Bookie.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;
    using Common;
    using Common.Model;
    using Core;
    using Core.Domains;
    using Core.Importer;
    using Core.Scraper;
    using Views;
    using static System.String;

    public class SourceDirectoryViewModel : NotifyBase
    {
        private readonly SourceDirectoryDomain _domain;
        private readonly Library _library;
        private ICommand _addCommand;
        private Importer _importer;
        private bool _progressReportingActive;
        private ICommand _removeCommand;
        private ICommand _scanCommand;
        private ICommand _scrapeCOmmand;
        private SourceDirectory _selectedSourceDirectory;
        private ObservableCollection<SourceDirectory> _sourceDirectories;
        public MainViewModel MainViewModel;

        public SourceDirectoryViewModel(MainViewModel v)
        {
            _library = new Library();
            MainViewModel = v;
            _domain = new SourceDirectoryDomain();
            SourceDirectories = new ObservableCollection<SourceDirectory>();

            Refresh();
        }

        public bool ProgressReportingActive
        {
            get { return _progressReportingActive; }
            set
            {
                _progressReportingActive = value;
                NotifyPropertyChanged("ProgressReportingActive");
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand
                       ?? (_removeCommand = new RelayCommand(p => Remove(), p => SelectedSourceDirectory != null));
            }
        }

        public ICommand ScanCommand
        {
            get
            {
                return _scanCommand
                       ?? (_scanCommand = new RelayCommand(p => Scan(), p => SelectedSourceDirectory != null));
            }
        }

        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(p => Add(), p => _domain != null)); }
        }

        public ICommand ScrapeCommand
        {
            get
            {
                return _scrapeCOmmand
                       ??
                       (_scrapeCOmmand =
                           new RelayCommand(p => Scrape(),
                               p => SelectedSourceDirectory != null && SelectedSourceDirectory.Books.Count > 0));
            }
        }

        public SourceDirectory SelectedSourceDirectory
        {
            get { return _selectedSourceDirectory; }
            set
            {
                _selectedSourceDirectory = value;
                NotifyPropertyChanged("SelectedSourceDirectory");
            }
        }

        public ObservableCollection<SourceDirectory> SourceDirectories
        {
            get { return _sourceDirectories; }
            set
            {
                _sourceDirectories = value;
                NotifyPropertyChanged("SourceDirectories");
            }
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

            string nickname = "";
            //NickName
            NickNameView view = new NickNameView();
            view.ViewModel.NickName = dialog.SelectedPath;
            if (view.ShowDialog() == true)
            {
                if (IsNullOrEmpty(view.ViewModel.NickName))
                {
                    nickname = dialog.SelectedPath;
                }
                else
                {
                    nickname = view.ViewModel.NickName;
                }
            }


            var source = new SourceDirectory {SourceDirectoryUrl = dialog.SelectedPath, NickName=nickname};

            if (_domain.Exists(source.SourceDirectoryUrl))
            {
                MessageBox.Show("Source directory already exists", "Information", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }
            source.EntityState = EntityState.Added;
            _domain.AddSourceDirectory(source);

            Refresh();
        }

        public void Remove()
        {
            SelectedSourceDirectory.EntityState = EntityState.Deleted;
            _domain.RemoveSourceDirectory(SelectedSourceDirectory);
            Refresh();
        }

        public void Scan()
        {
            bool covers;
            bool subdirectories;
            var view = new ConfirmImportView();
            if (view.ShowDialog() == true)
            {
                covers = view._viewModel.GenerateCovers;
                subdirectories = view._viewModel.SubDirectories;
            }
            else
            {
                return;
            }

            SelectedSourceDirectory.DateLastImported = DateTime.Now;
            SelectedSourceDirectory.EntityState = EntityState.Modified;
            _domain.UpdateSourceDirectory(SelectedSourceDirectory);

            ProgressReportingActive = true;

            _importer = new Importer(SelectedSourceDirectory);
            _importer.BookChanged += MainViewModel.i_BookChanged;
            _importer.Worker.RunWorkerCompleted += _worker_RunWorkerCompleted;

            _importer.ScanSource(subdirectories, covers);
            _importer.ProgressComplete += delegate { ProgressReportingActive = false; };

            Refresh();
            _library.CleanImages();
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Refresh();
        }

        public void Scrape()
        {
            bool covers;
            bool rescrape;
            var view = new ConfirmScrapeView();
            if (view.ShowDialog() == true)
            {
                covers = view._viewModel.GenerateCovers;
                rescrape = view._viewModel.ReScrape;
            }
            else
            {
                return;
            }

            var scraper = new Scraper();

            ProgressReportingActive = true;

            scraper.BookChanged += MainViewModel.i_BookChanged;
            scraper.Worker.RunWorkerCompleted += _worker_RunWorkerCompleted;

            scraper.Scrape(SelectedSourceDirectory, MainViewModel.Books.Cast<Book>().ToList(), covers, rescrape);
            scraper.ProgressComplete += delegate { ProgressReportingActive = false; };
            Refresh();
            _library.CleanImages();
        }
    }
}