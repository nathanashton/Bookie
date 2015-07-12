namespace Bookie.ViewModels
{
    using Bookie.Common;
    using Bookie.Common.Model;
    using Bookie.Core.Domains;
    using Bookie.UserControls;
    using Bookie.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    public class MainViewModel : NotifyBase, IProgressSubscriber
    {
        private bool _toggleScraped;

        public bool ToggleScraped
        {
            get
            {
                return _toggleScraped;
            }
            set
            {
                _toggleScraped = value;
                NotifyPropertyChanged("ToggleScraped");
                if (value)
                {
                    ScrapedColor = new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    ScrapedColor = new SolidColorBrush(Colors.White);
                }
                ApplyToggleFilter();
                NotifyPropertyChanged("Books");
            }
        }


        public bool ToggleFavouriteBook
        {
            get
            {
                if (SelectedBook != null)
                {
                    ToggleFavouriteColor = SelectedBook.Favourite ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Black);
                    return SelectedBook.Favourite;
                }
                return false;
            }
            set
            {
                SelectedBook.Favourite = value;
                ToggleFavouriteColor = value ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Black);
                NotifyPropertyChanged("ToggleFavouriteBook");
                BookDomain.SetUnchanged(SelectedBook);
                SelectedBook.EntityState = EntityState.Modified;
                _bookDomain.UpdateBook(SelectedBook);


                if (ToggleFavourite)
                {
                    ApplyToggleFilter();
                    NotifyPropertyChanged("Books");
                }

            }
        }

        private Brush _toggleFavouriteColor;

        public Brush ToggleFavouriteColor
        {
            get
            {
                return _toggleFavouriteColor;
            }
            set
            {
                _toggleFavouriteColor = value;
                NotifyPropertyChanged("ToggleFavouriteColor");
            }
        }

        private Brush _starColor;

        public Brush StarColor
        {
            get
            {
                return _starColor;
            }
            set
            {
                _starColor = value;
                NotifyPropertyChanged("StarColor");
            }
        }

        private Brush _scrapedColor;

        public Brush ScrapedColor
        {
            get
            {
                return _scrapedColor;
            }
            set
            {
                _scrapedColor = value;
                NotifyPropertyChanged("ScrapedColor");
            }
        }

        private bool _toggleFavourite;

        public bool ToggleFavourite
        {
            get
            {
                return _toggleFavourite;
            }
            set
            {
                _toggleFavourite = value;
                NotifyPropertyChanged("ToggleFavourite");
                if (value)
                {
                    StarColor = new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    StarColor = new SolidColorBrush(Colors.White);
                }
                ApplyToggleFilter();
                NotifyPropertyChanged("Books");
            }
        }

        private ICommand _viewLog;
        private bool _showProgress;
        private bool _progressReportingActive;
        private string _progressText;
        private bool _cancelled;

        private int _progressPercentage;
        private string _filter;

        private int _tileWidth;

        private int _tileHeight;
        private string _operationName;

        private string _progressBarText;
        private Book _selectedBook;

        private Visibility _leftPane;

        private Visibility _filterBoxVisibility;
        private Visibility _rightPane;
        private ICollectionView _books;
        private ICommand _openPDFCommand;
        private ICommand _leftPaneCommand;
        private ICommand _refreshCommand;
        private ICommand _listViewCommand;

        private ICommand _rightPaneCommand;

        private ICommand _cancelProgressCommand;

        private ICommand _settingsViewCommand;

        private ICommand _tileViewCommand;

        private ICommand _sourceViewCommand;
        private List<Publisher> _allPublishers;
        private readonly BookDomain _bookDomain;

        private string _selectedSort;
        private ObservableCollection<Author> _authorsTV;
        private ObservableCollection<Publisher> _publishersTV;

        private ObservableCollection<string> _sortList;

        public Visibility FilterBoxVisibility
        {
            get
            {
                return _filterBoxVisibility;
            }
            set
            {
                _filterBoxVisibility = value;
                NotifyPropertyChanged("FilterBoxVisibility");
            }
        }

        public ObservableCollection<string> SortList
        {
            get
            {
                return _sortList;
            }
            set
            {
                _sortList = value;
                NotifyPropertyChanged("SortList");
            }
        }

        public string SelectedSort
        {
            get
            {
                return _selectedSort;
            }
            set
            {
                _selectedSort = value;
                NotifyPropertyChanged("SelectedSort");

                if (_selectedSort != null)
                {
                    SortBooks();
                }
            }
        }

        public Window Window;

        public UIElement _bookView;

        public ObservableCollection<Book> AllBooks;

        public BookTiles BookTiles;

        public BookDetails BookDetails;

        public PDFViewer PdfViewer;

        public bool ShowProgress
        {
            get
            {
                return _showProgress;
            }
            set
            {
                _showProgress = value;
                NotifyPropertyChanged("ShowProgress");
            }
        }

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

        public Visibility LeftPane
        {
            get
            {
                return _leftPane;
            }
            set
            {
                _leftPane = value;
                NotifyPropertyChanged("LeftPane");
            }
        }

        public Visibility RightPane
        {
            get
            {
                return _rightPane;
            }
            set
            {
                _rightPane = value;
                NotifyPropertyChanged("RightPane");
            }
        }

        public ICollectionView Books
        {
            get
            {
                return _books;
            }
            set
            {
                _books = value;
                NotifyPropertyChanged("Books");
            }
        }

        public ObservableCollection<Publisher> PublishersList
        {
            get
            {
                return _publishersTV;
            }
            set
            {
                _publishersTV = value;
                NotifyPropertyChanged("PublishersList");
            }
        }

        public ObservableCollection<Author> AuthorsList
        {
            get
            {
                return _authorsTV;
            }
            set
            {
                _authorsTV = value;
                NotifyPropertyChanged("AuthorsList");
            }
        }

        public string Filter
        {
            get
            {
                return String.IsNullOrEmpty(_filter) ? "" : _filter;
            }
            set
            {
                _filter = value;
                NotifyPropertyChanged("Filter");
                Books.Filter = ApplyTextFilter;
                Books.Refresh();
            }
        }

        private Publisher _publisherFilter;

        public Publisher PublisherFilter
        {
            get
            {
                return _publisherFilter;
            }
            set
            {
                _publisherFilter = value;
                NotifyPropertyChanged("PublisherFilter");
                Books.Filter = ApplyPublisherFilter;
                Books.Refresh();
            }
        }

        private SourceDirectory _sourceDirectoryFilter;

        public SourceDirectory SourceDirectoryFilter
        {
            get
            {
                return _sourceDirectoryFilter;
            }
            set
            {
                _sourceDirectoryFilter = value;
                NotifyPropertyChanged("SourceDirectoryFilter");
                Books.Filter = ApplySourceDirectoryFilter;
                Books.Refresh();
            }
        }

        private Author _authorFilter;

        public Author AuthorFilter
        {
            get
            {
                return _authorFilter;
            }
            set
            {
                _authorFilter = value;
                NotifyPropertyChanged("AuthorFilter");
                Books.Filter = ApplyAuthorFilter;
                Books.Refresh();
            }
        }

        public UIElement BookView
        {
            get
            {
                return _bookView;
            }
            set
            {
                _bookView = value;
                NotifyPropertyChanged("BookView");
                FilterBoxVisibility = Equals(_bookView, PdfViewer) ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private SourceDirectoryDomain _sourceDomain = new SourceDirectoryDomain();

        public Book SelectedBook
        {
            get
            {
                return _selectedBook;
            }
            set
            {
                _selectedBook = value;
                if (value != null)
                {
                    PdfViewer.PDFUrl = SelectedBook;
                }
                NotifyPropertyChanged("SelectedBook");
                NotifyPropertyChanged("SelectedBookEmpty");
                NotifyPropertyChanged("ToggleFavouriteBook");
            }
        }

        public int TileHeight
        {
            get
            {
                return _tileHeight;
            }
            set
            {
                _tileHeight = value;
                NotifyPropertyChanged("TileHeight");
            }
        }

        public bool SelectedBookEmpty
        {
            get
            {
                return SelectedBook != null;
            }
        }

        public string BooksCount
        {
            get
            {
                return "Found " + Books.Cast<Book>().Count() + " results";
            }
        }

        public int TileWidth
        {
            get
            {
                return _tileWidth;
            }
            set
            {
                _tileWidth = value;
                _tileHeight = Convert.ToInt32(value * 1.4);
                NotifyPropertyChanged("TileWidth");
                NotifyPropertyChanged("TileHeight");
                NotifyPropertyChanged("StarSize");
                Properties.Settings.Default.TileWidth = value;
            }
        }

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

        public ICommand CancelProgressCommand
        {
            get
            {
                return _cancelProgressCommand
                       ?? (_cancelProgressCommand = new RelayCommand(p => CancelProgress(), p => true));
            }
        }

        public ICommand ViewLog
        {
            get
            {
                return _viewLog
                       ?? (_viewLog = new RelayCommand(p => ViewLogWindow(), p => true));
            }
        }

        public ICommand OpenPDFCommand
        {
            get
            {
                return _openPDFCommand
                       ?? (_openPDFCommand = new RelayCommand(p => ChangeToPdfView(), p => SelectedBook != null));
            }
        }

        public ICommand LeftPaneCommand
        {
            get
            {
                return _leftPaneCommand
                       ?? (_leftPaneCommand = new RelayCommand(p => LeftPaneToggle(), p => true));
            }
        }

        public ICommand RightPaneCommand
        {
            get
            {
                return _rightPaneCommand
                       ?? (_rightPaneCommand = new RelayCommand(p => RightPaneToggle(), p => true));
            }
        }

        public ICommand SettingsViewCommand
        {
            get
            {
                return _settingsViewCommand
                       ?? (_settingsViewCommand = new RelayCommand(p => ShowSettingsView(), p => true));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                       ?? (_refreshCommand = new RelayCommand(p => RefreshAllBooks(), p => AllBooks != null));
            }
        }

        public ICommand ListViewCommand
        {
            get
            {
                return _listViewCommand
                       ?? (_listViewCommand = new RelayCommand(p => SwitchToDetailsView(), p => true));
            }
        }

        public ICommand TileViewCommand
        {
            get
            {
                return _tileViewCommand
                       ?? (_tileViewCommand = new RelayCommand(p => SwitchToTilesView(), p => true));
            }
        }

        public ICommand SourceViewCommand
        {
            get
            {
                return _sourceViewCommand
                       ?? (_sourceViewCommand = new RelayCommand(p => SourceDirectoryView(), p => true));
            }
        }

        private ObservableCollection<SourceDirectory> _sourceDirectories;

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

        public List<Publisher> AllPublishers
        {
            get
            {
                return _allPublishers;
            }
            set
            {
                _allPublishers = value;
                NotifyPropertyChanged("AllPublishers");
            }
        }

        public List<Book> BooksFromSplash { get; set; }

        public MainViewModel()
        {
            SourceDirectories = new ObservableCollection<SourceDirectory>();
            StarColor = new SolidColorBrush(Colors.White);
            ScrapedColor = new SolidColorBrush(Colors.White);
            ToggleFavouriteColor = new SolidColorBrush(Colors.Black);

            _bookDomain = new BookDomain();
            BookTiles = new BookTiles();
            BookDetails = new BookDetails();
            PdfViewer = new PDFViewer();

            //  var savedView = AppConfig.LoadSetting("SavedView");
            //switch (savedView)
            //{
            //    case "Tiles":
            //        BookView = BookTiles;
            //        break;

            //    case "Details":
            //        BookView = BookDetails;
            //        break;

            //    default:
            //        BookView = new BookTiles();
            //        break;
            //}
            BookView = BookTiles;
            ProgressService.RegisterSubscriber(this);

            var sortt = new List<string>
                            {
                                "Title [A-Z]",
                                "Title [Z-A]",
                                "Date Published [Newest]",
                                "Date Published [Oldest]",
                                "Date Added [Newest]",
                                "Date Added [Oldest]"
                            };

            SortList = new ObservableCollection<string>(sortt);
            RefreshAllBooks();
            RefreshPublishersAndAuthors();
            SelectedSort = "Title [A-Z]";
        }

        public void _progress_ProgressStarted(object sender, EventArgs e)
        {
            OperationName = "Starting...";
            ProgressText = "";
            ProgressPercentage = 0;
            ProgressBarText = "";
            ProgressReportingActive = true;
            ShowProgress = true;
        }

        public void _progress_ProgressCompleted(object sender, EventArgs e)
        {
            OperationName = "Starting...";
            ProgressPercentage = 0;
            ProgressText = "";
            ProgressBarText = "";
            ProgressReportingActive = false;
            ShowProgress = false;
        }

        public void _progress_ProgressChanged(object sender, ProgressWindowEventArgs e)
        {
            OperationName = e.OperationName;
            ProgressBarText = e.ProgressBarText;
            ProgressText = e.ProgressText;
            ProgressPercentage = e.ProgressPercentage;
        }

        private void Books_CollectionChanged(
            object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("BooksCount");
        }

        public void ChangeToPdfView()
        {
            if (SelectedBook == null)
            {
                return;
            }
            LeftPane = Visibility.Collapsed;
            RightPane = Visibility.Collapsed;
            PdfViewer.OpenPDF(SelectedBook.BookFile.FullPathAndFileNameWithExtension);
            BookView = PdfViewer;
        }

        private void CancelProgress()
        {
            ProgressService.Cancel();
        }

        private void ViewLogWindow()
        {
            var log = new LogWindow();
            log.ShowDialog();
        }

        public void i_BookChanged(object sender, BookEventArgs e)
        {
            if (e.Book == null)
            {
                return;
            }
            switch (e.State)
            {
                case (BookEventArgs.BookState.Added):
                    var bookExistsAdded =
                        AllBooks.Any(
                            b =>
                            b.BookFile.FullPathAndFileNameWithExtension
                            == e.Book.BookFile.FullPathAndFileNameWithExtension);
                    if (!bookExistsAdded)
                    {
                        AllBooks.Add(e.Book);
                    }
                    NotifyPropertyChanged("BooksCount");
                    break;

                case (BookEventArgs.BookState.Removed):
                    var bookExistsRemoved =
                        AllBooks.Any(
                            b =>
                            b.BookFile.FullPathAndFileNameWithExtension
                            == e.Book.BookFile.FullPathAndFileNameWithExtension);
                    if (bookExistsRemoved)
                    {
                        AllBooks.Remove(e.Book);
                    }
                    NotifyPropertyChanged("BooksCount");
                    break;

                case (BookEventArgs.BookState.Updated): //Remove book from list and re-add it
                    var bookExistsUpdated =
                        AllBooks.FirstOrDefault(
                            b =>
                            b.BookFile.FullPathAndFileNameWithExtension
                            == e.Book.BookFile.FullPathAndFileNameWithExtension);
                    if (bookExistsUpdated != null)
                    {
                        var index = AllBooks.IndexOf(bookExistsUpdated);
                        AllBooks.Remove(bookExistsUpdated);
                        AllBooks.Insert(index, e.Book);
                    }
                    else
                    {
                        AllBooks.Add(e.Book);
                    }
                    NotifyPropertyChanged("BooksCount");
                    break;
            }
        }

        private bool ApplyTextFilter(object item)
        {
            var book = item as Book;
            return book != null && book.Title.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0 && book.SourceDirectory.SourceDirectoryUrl == SourceDirectoryFilter.SourceDirectoryUrl;
        }

        private bool ApplyPublisherFilter(object item)
        {
            var book = item as Book;
            if (PublisherFilter != null && PublisherFilter.Name == "All Publishers")
            {
                return true;
            }
            return book != null && book.Publishers.Any(x => x.Name == PublisherFilter.Name);
        }

        private bool ApplyAuthorFilter(object item)
        {
            var book = item as Book;
            if (AuthorFilter != null && AuthorFilter.FirstName == "All Authors")
            {
                return true;
            }

            return book != null && book.Authors.Any(x => x.FullName == AuthorFilter.FullName);
        }

        private bool ApplySourceDirectoryFilter(object item)
        {
            var book = item as Book;
            if (SourceDirectoryFilter == null)
            {
                SourceDirectoryFilter = new SourceDirectory { SourceDirectoryUrl = "All Sources" };
            }
            if (SourceDirectoryFilter.SourceDirectoryUrl == "All Sources")
            {
                return true;
            }
            return book != null && book.SourceDirectory.SourceDirectoryUrl == SourceDirectoryFilter.SourceDirectoryUrl;
        }

        private void SwitchToTilesView()
        {
            if (Equals(BookView, PdfViewer))
            {
                LeftPane = Visibility.Visible;
                RightPane = Visibility.Visible;
            }
            BookView = BookTiles;
        }

        private void SwitchToDetailsView()
        {
            if (Equals(BookView, PdfViewer))
            {
                LeftPane = Visibility.Visible;
                RightPane = Visibility.Visible;
            }
            BookView = BookDetails;
        }

        private void ApplyToggleFilter()
        {
            IEnumerable<Book> filteredBooks = _bookDomain.GetAllBooks().ToList();
            if (ToggleScraped)
            {
                filteredBooks = filteredBooks.Where(x => x.Scraped).ToList();
            }
            if (ToggleFavourite)
            {
                filteredBooks = filteredBooks.Where(x => x.Favourite).ToList();
            }
            AllBooks = new ObservableCollection<Book>(filteredBooks);

            Books = CollectionViewSource.GetDefaultView(AllBooks);
            Books.Refresh();
        }

        private void SortBooks()
        {
            switch (SelectedSort)
            {
                case "Title [A-Z]":
                    Books.SortDescriptions.Clear();

                    Books.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                    break;

                case "Title [Z-A]":
                    Books.SortDescriptions.Clear();
                    Books.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Descending));
                    break;

                case "Date Published [Newest]":
                    Books.SortDescriptions.Clear();
                    Books.SortDescriptions.Add(new SortDescription("DatePublished", ListSortDirection.Descending));
                    break;

                case "Date Published [Oldest]":
                    Books.SortDescriptions.Clear();
                    Books.SortDescriptions.Add(new SortDescription("DatePublished", ListSortDirection.Ascending));
                    break;

                case "Date Added [Newest]":
                    Books.SortDescriptions.Clear();
                    Books.SortDescriptions.Add(new SortDescription("CreatedDateTime", ListSortDirection.Descending));
                    break;

                case "Date Added [Oldest]":
                    Books.SortDescriptions.Clear();
                    Books.SortDescriptions.Add(new SortDescription("CreatedDateTime", ListSortDirection.Ascending));
                    break;
            }
        }

        public async void RefreshAllBooks()
        {
            var b = await _bookDomain.GetAllAsync();
            AllBooks = b != null ? new ObservableCollection<Book>(b) : new ObservableCollection<Book>();
            Books = CollectionViewSource.GetDefaultView(AllBooks);
            Books.CollectionChanged += Books_CollectionChanged;

            SourceDirectories.Clear();
            SourceDirectories = new ObservableCollection<SourceDirectory>(_sourceDomain.GetAllSourceDirectories().ToList());
            SourceDirectories.Insert(0, new SourceDirectory { SourceDirectoryUrl = "All Sources" });
            SourceDirectoryFilter = SourceDirectories[0];
        }

        public void RefreshPublishersAndAuthors()
        {
            var p = new PublisherDomain();
            var all = p.GetAllPublishers();
            if (all != null)
            {
                all.Insert(0, new Publisher { Name = "All Publishers" });
                if (Books != null)
                {
                    PublishersList = new ObservableCollection<Publisher>(all);
                }
            }

            var a = new AuthorDomain();
            var allAuthors = a.GetAllAuthors();
            if (allAuthors == null)
            {
                return;
            }
            allAuthors.Insert(0, new Author { FirstName = "All Authors" });

            if (Books != null)
            {
                AuthorsList = new ObservableCollection<Author>(allAuthors);
            }
        }

        private void SourceDirectoryView()
        {
            var sourceDirectoryView = new SourceDirectoryView(this);
            sourceDirectoryView.Show();
        }

        public void ShowSettingsView()
        {
            var settingsView = new SettingsView();
            settingsView.ShowDialog();
        }

        public void ProgressSubscriber(ProgressWindowEventArgs e)
        {
            ProgressPercentage = e.ProgressPercentage;
        }

        public void LeftPaneToggle()
        {
            LeftPane = LeftPane == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public void RightPaneToggle()
        {
            RightPane = RightPane == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}