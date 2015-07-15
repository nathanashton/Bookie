namespace Bookie.Core.Scraper
{
    using Bookie.Common;
    using Bookie.Common.Model;
    using Bookie.Core.Domains;
    using Bookie.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    public class Scraper : IProgressPublisher
    {
        private readonly IsbnGuesser _guesser = new IsbnGuesser();

        private readonly BookDomain _bookDomain = new BookDomain();

        private readonly ICoverImageDomain _coverImageDomain = new CoverImageDomain();

        private SourceDirectory _sourceDirectory;

        public ProgressWindowEventArgs ProgressArgs { get; set; }

        private List<Book> _booksToScrape;

        private bool _noInternet;

        private bool _generateCovers;

        private bool _rescrape;

        public readonly BackgroundWorker Worker = new BackgroundWorker();

        private readonly IBookScraper _scraper = new GoogleScraper();

        public event EventHandler<BookEventArgs> BookChanged;

        private ObservableCollection<SearchResult> _results;

        public event EventHandler<ProgressWindowEventArgs> ProgressChanged;

        public event EventHandler<EventArgs> ProgressComplete;

        public event EventHandler<EventArgs> ProgressStarted;

        public void OnBookChanged(Book book, BookEventArgs.BookState bookState, int? progress)
        {
            if (BookChanged != null)
            {
                BookChanged(this, new BookEventArgs { Book = book, State = bookState, Progress = progress });
            }
        }

        public Scraper()
        {
            ProgressService.RegisterPublisher(this);
            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += Worker_DoWork;
            Worker.ProgressChanged += Worker_ProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            Worker.WorkerReportsProgress = true;
            ProgressArgs = new ProgressWindowEventArgs();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_noInternet)
            {
                MessagingService.ShowErrorMessage("No internet connection was found. The scrape was cancelled.", false);
            }
            _noInternet = false;
            _sourceDirectory.DateLastScanned = DateTime.Now;
            _sourceDirectory.EntityState = EntityState.Modified;
            new SourceDirectoryDomain().UpdateSourceDirectory(_sourceDirectory);
            OnProgressComplete();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var book = (Book)e.UserState;
            OnBookChanged(book, BookEventArgs.BookState.Updated, e.ProgressPercentage);
            ProgressArgs.OperationName = "Scraping Books";
            ProgressArgs.ProgressBarText = e.ProgressPercentage + "%";
            ProgressArgs.ProgressPercentage = Convert.ToInt32(e.ProgressPercentage);
            ProgressArgs.ProgressText = book.Title;
            OnProgressChange(ProgressArgs);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //TODO Needs work as images arent being generated if no isbn if found
            for (var index = 0; index < _booksToScrape.Count; index++)
            {
                if (Worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                var book = _booksToScrape[index];
                if (_rescrape == false && book.Scraped)
                {
                    continue;
                } 

                book.Isbn = _guesser.GuessBookIsbn(book.BookFile.FullPathAndFileNameWithExtension);
                if (String.IsNullOrEmpty(book.Isbn))
                {
                    //Couldnt find valid isbn
                    continue;
                }
                SearchResult b;
                try
                {
                    _results = _scraper.SearchBooks(book.Isbn);
                }
                catch (BookieException)
                {
                    Logger.Log.Error("No internet connection while scraping. Terminated");
                    e.Cancel = true;
                    _noInternet = true;
                    return;
                }

                if (_results != null && _results.Count > 0)
                {
                    b = _results.FirstOrDefault(x => x.Book != null);
                    if (b == null)
                    {
                        continue;
                    }
                    book.Isbn = book.Isbn;
                    book.Title = b.Book.Title;
                    book.Abstract = b.Book.Abstract ?? "";
                    book.Pages = b.Book.Pages;
                    book.DatePublished = b.Book.DatePublished;
                    book.Scraped = true;

                    if (b.Book.Authors != null)
                    {
                        book.Authors = b.Book.Authors;
                    }
                    if (b.Book.Publishers != null)
                    {
                        book.Publishers = b.Book.Publishers;
                    }
                }
                else
                {
                    book.Isbn = IsbnGuesser.Isbn13to10(book.Isbn);
                    if (String.IsNullOrEmpty(book.Isbn))
                    {
                        book.Scraped = false;
                        continue;
                    }
                    _results = _scraper.SearchBooks(book.Isbn);
                    if (_results == null || _results.Count <= 0)
                    {
                        continue;
                    }
                    b = _results.FirstOrDefault(x => x.Book != null);
                    if (b == null)
                    {
                        continue;
                    }
                    book.Isbn = book.Isbn;
                    book.Title = b.Book.Title;
                    book.Abstract = b.Book.Abstract ?? "";
                    book.Pages = b.Book.Pages;
                    book.DatePublished = b.Book.DatePublished;
                }

                if (_generateCovers)
                {
                    book.CoverImage = _coverImageDomain.GenerateCoverImageFromPdf(book);
                }
                else
                {
                    book.CoverImage.EntityState = EntityState.Unchanged;
                }

                var publishers = b.Publishers.ToList();
                var authors = b.Authors.ToList();

                foreach (var publisher in publishers)
                {
                    publisher.EntityState = EntityState.Added;
                    book.Publishers.Add(publisher);
                }

                foreach (var author in authors)
                {
                    author.EntityState = EntityState.Added;
                    book.Authors.Add(author);
                }

                book.BookFile.EntityState = EntityState.Unchanged;
                book.BookHistory.EntityState = EntityState.Unchanged;
                book.SourceDirectory.EntityState = EntityState.Unchanged;
                book.EntityState = EntityState.Modified;

                _bookDomain.UpdateBook(book);

                var percentage = Utils.CalculatePercentage(index + 1, 1, _booksToScrape.Count);
                Worker.ReportProgress(percentage, book);
            }
        }

        public void Scrape(SourceDirectory source, List<Book> books, bool generateCovers, bool reScrape)
        {
            _sourceDirectory = source;
            _booksToScrape = books;
            if (_booksToScrape.Count == 0)
            {
                return;
            }
            _generateCovers = generateCovers;
            _rescrape = reScrape;
            _noInternet = false;
            OnProgressStarted();
            Worker.RunWorkerAsync();
        }

        private void OnProgressComplete()
        {
            if (ProgressComplete != null)
            {
                ProgressComplete(this, null);
            }
        }

        private void OnProgressStarted()
        {
            if (ProgressStarted != null)
            {
                ProgressStarted(this, null);
            }
        }

        private void OnProgressChange(ProgressWindowEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }

        public void ProgressCancel()
        {
            if (Worker.IsBusy)
            {
                Worker.CancelAsync();
            }
        }
    }
}