namespace Bookie.Core.Importer
{
    using Bookie.Common.Model;
    using global::Bookie.Common;
    using global::Bookie.Core.Domains;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    public class Importer : IProgressPublisher
    {
        private readonly SourceDirectory _source;
        private List<string> _foundPdfFiles;
        public readonly BackgroundWorker Worker;
        private readonly BookDomain _bookDomain = new BookDomain();

        public ProgressWindowEventArgs ProgressArgs { get; set; }

        private bool _generateCovers;

        public event EventHandler<BookEventArgs> BookChanged;

        public event EventHandler<ProgressWindowEventArgs> ProgressChanged;

        public event EventHandler<EventArgs> ProgressComplete;

        public event EventHandler<EventArgs> ProgressStarted;

        private int _booksImported;

        private int _booksExisted;

        public Importer(SourceDirectory source)
        {
            ProgressService.RegisterPublisher(this);
            _source = source;
            _source.Books = new HashSet<Book>();
            _foundPdfFiles = new List<string>();
            Worker = new BackgroundWorker();
            Worker.DoWork += _worker_DoWork;
            Worker.ProgressChanged += _worker_ProgressChanged;
            Worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            ProgressArgs = new ProgressWindowEventArgs();
            _booksImported = 0;
            _booksExisted = 0;
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnProgressComplete();
            Logger.Log.Info(String.Format("Import Complete: Books Imported {0}: Already Existed {1}.", _booksImported, _booksExisted));
            MessagingService.ShowMessage(String.Format("{0} Books imported.{1}{2} Books already existed.", _booksImported, Environment.NewLine, _booksExisted));
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState == null)
            {
                OnBookChanged(null, BookEventArgs.BookState.Added, e.ProgressPercentage);
                ProgressArgs.OperationName = "Importing Books";
                ProgressArgs.ProgressBarText = e.ProgressPercentage + "%";
                ProgressArgs.ProgressPercentage = Convert.ToInt32(e.ProgressPercentage);
                ProgressArgs.ProgressText = "Book Exists";
            }
            else
            {
                var book = (Book)e.UserState;
                OnBookChanged(book, BookEventArgs.BookState.Added, e.ProgressPercentage);
                ProgressArgs.OperationName = "Importing Books";
                ProgressArgs.ProgressBarText = e.ProgressPercentage + "%";
                ProgressArgs.ProgressPercentage = Convert.ToInt32(e.ProgressPercentage);
                ProgressArgs.ProgressText = book.Title;
            }
            OnProgressChange(ProgressArgs);
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _booksImported = 0;
            _booksExisted = 0;
            for (var index = 0; index < _foundPdfFiles.Count; index++)
            {
                if (Worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                var foundBook = _foundPdfFiles[index];
                _source.EntityState = EntityState.Unchanged;

                var book = new Book();

                book.Title = Path.GetFileNameWithoutExtension(foundBook);
                book.BookFile.FileNameWithExtension = Path.GetFileName(foundBook);
                book.BookFile.FullPathAndFileNameWithExtension = foundBook;
                book.BookFile.FileExtension = Path.GetExtension(foundBook);
                book.BookFile.FileSizeBytes = new FileInfo(foundBook).Length;
                book.BookFile.EntityState = EntityState.Added;

                book.SourceDirectory = new SourceDirectory { Id = _source.Id, EntityState = EntityState.Unchanged };
                book.BookHistory.DateImported = DateTime.Now;
                book.BookHistory.EntityState = EntityState.Added;
                book.CoverImage = new CoverImage();

                string savedImageUrl = "";
                if (_generateCovers)
                {
                    savedImageUrl = GetPdfImage.LoadImage(book, 1);
                }

                book.CoverImage.FileNameWithExtension = Path.GetFileName(savedImageUrl);
                book.CoverImage.FullPathAndFileNameWithExtension = Globals.CoverImageFolder
                                                                       + Path.GetFileNameWithoutExtension(
                                                                           savedImageUrl) + ".jpg";
                book.CoverImage.FileExtension = ".jpg";
                book.CoverImage.EntityState = EntityState.Added;

                var percentage = Utils.CalculatePercentage(index + 1, 1, _foundPdfFiles.Count);

                book.EntityState = EntityState.Added;

                if (_bookDomain.Exists(book.BookFile.FullPathAndFileNameWithExtension))
                {
                    _booksExisted++;
                    Logger.Log.Info("Importer Skipped: " + book.Title + " already exists.");
                }
                else
                {
                    _bookDomain.AddBook(book);
                    Logger.Log.Info("Imported: " + book.Title);

                    _booksImported++;
                    Worker.ReportProgress(percentage, book);
                }
            }
        }

        public void ScanSource(bool includeSubDirectories, bool generateCovers)
        {
            if (_source == null)
            {
                return;
            }
            _generateCovers = generateCovers;
            var option = includeSubDirectories == false ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            _foundPdfFiles = Directory.GetFiles(_source.SourceDirectoryUrl, FileTypes.PDF, option).ToList();
            Logger.Log.Debug(String.Format("Found {0} files in {1} with an extension of {2}", _foundPdfFiles.Count(),_source.SourceDirectoryUrl , FileTypes.PDF));
  
            Logger.Log.Info(
                String.Format(
                    "Importing from {0}: Subdirectories {1}: Generate Covers {2}",
                    _source.SourceDirectoryUrl,
                    includeSubDirectories,
                    generateCovers));
            OnProgressStarted();
            Worker.RunWorkerAsync();
        }

        public void OnBookChanged(Book book, BookEventArgs.BookState bookState, int? progress)
        {
            if (BookChanged != null)
            {
                BookChanged(this, new BookEventArgs { Book = book, State = bookState, Progress = progress });
            }
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