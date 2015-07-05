namespace Bookie.Core.Scraper
{
    using Bookie.Common;
    using Bookie.Common.Model;
    using Bookie.Core.Domains;
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

        private SourceDirectory _sourceDirectory;
        public ProgressWindowEventArgs ProgressArgs { get; set; }

        private List<Book> _booksToScrape;

        public readonly BackgroundWorker _worker = new BackgroundWorker();
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
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += Worker_DoWork;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            _worker.WorkerReportsProgress = true;
            ProgressArgs = new ProgressWindowEventArgs();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
            for (var index = 0; index < _booksToScrape.Count; index++)
            {
                if (_worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                var book = _booksToScrape[index];
                if (book.Scraped)
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
                _results = _scraper.SearchBooks(book.Isbn);
                if (_results != null && _results.Count > 0)
                {
                    b = _results.FirstOrDefault(x => x.Book != null);
                    if (b == null)
                    {
                        continue;
                    }
                    book.Isbn = book.Isbn;
                    book.Title = b.Book.Title;
                    book.Abstract = b.Book.Abstract;
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
                    book.Abstract = b.Book.Abstract;
                    book.Pages = b.Book.Pages;
                    book.DatePublished = b.Book.DatePublished;
                }

                if (!File.Exists(book.CoverImage.FullPathAndFileNameWithExtension))
                {
                    //  var savedImageUrl = GetPdfImage.LoadImage(book, 1);

                    //CoverImage c = new CoverImage();
                    //c.Id = book.CoverImage.Id;
                    //c.FileNameWithExtension = Path.GetFileName(savedImageUrl);
                    //c.FullPathAndFileNameWithExtension = Globals.CoverImageFolder
                    //                                                       + Path.GetFileNameWithoutExtension(
                    //                                                           savedImageUrl);
                    //c.FileExtension = ".jpg";
                    //c.FileSizeBytes = new FileInfo(savedImageUrl).Length;

                    //Update Book Cover image in repository
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

                book.CoverImage.EntityState = EntityState.Unchanged;
                book.BookFile.EntityState = EntityState.Unchanged;
                book.BookHistory.EntityState = EntityState.Unchanged;
                book.SourceDirectory.EntityState = EntityState.Unchanged;
                book.EntityState = EntityState.Modified;

                _bookDomain.UpdateBook(book);

                var percentage = Utils.CalculatePercentage(index + 1, 1, _booksToScrape.Count);
                _worker.ReportProgress(percentage, book);
            }
        }

        public void Scrape(SourceDirectory source, List<Book> books)
        {
            _sourceDirectory = source;
            _booksToScrape = books;
            if (_booksToScrape.Count == 0)
            {
                return;
            }

            OnProgressStarted();
            _worker.RunWorkerAsync();
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
            if (_worker.IsBusy)
            {
                _worker.CancelAsync();
            }
        }
    }
}