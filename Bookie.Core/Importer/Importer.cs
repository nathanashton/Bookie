namespace Bookie.Core.Importer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using Common;
    using Common.Model;
    using Domains;
    using Interfaces;

    public class Importer : IProgressPublisher
    {
        private readonly BookDomain _bookDomain = new BookDomain();
        private readonly ICoverImageDomain _coverImageDomain = new CoverImageDomain();
        private readonly SourceDirectory _source;
        public readonly BackgroundWorker Worker;
        private int _booksExisted;
        private int _booksImported;
        private List<string> _foundPdfFiles;
        private bool _generateCovers;

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

        public ProgressWindowEventArgs ProgressArgs { get; set; }
        public event EventHandler<ProgressWindowEventArgs> ProgressChanged;
        public event EventHandler<EventArgs> ProgressComplete;
        public event EventHandler<EventArgs> ProgressStarted;

        public void ProgressCancel()
        {
            if (Worker.IsBusy)
            {
                Worker.CancelAsync();
            }
        }

        public event EventHandler<BookEventArgs> BookChanged;

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnProgressComplete();
            Logger.Log.Info($"Import Complete: Books Imported {_booksImported}: Already Existed {_booksExisted}.");
            MessagingService.ShowMessage(
                $"{_booksImported} Books imported.{Environment.NewLine}{_booksExisted} Books already existed.");
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
                var book = (Book) e.UserState;
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

                var book = new Book
                {
                    Title = Path.GetFileNameWithoutExtension(foundBook),
                    Abstract = "",
                    BookFile =
                    {
                        FileNameWithExtension = Path.GetFileName(foundBook),
                        FullPathAndFileNameWithExtension = foundBook,
                        FileExtension = Path.GetExtension(foundBook),
                        FileSizeBytes = new FileInfo(foundBook).Length,
                        EntityState = EntityState.Added
                    },
                    SourceDirectory = new SourceDirectory {Id = _source.Id, EntityState = EntityState.Unchanged},
                    BookHistory =
                    {
                        DateImported = DateTime.Now,
                        EntityState = EntityState.Added
                    },
                    CoverImage = new CoverImage()
                };


                if (_generateCovers)
                {
                    book.CoverImage = _coverImageDomain.GenerateCoverImageFromPdf(book);
                }
                else
                {
                    book.CoverImage = CoverImageDomain.EmptyCoverImage();
                    book.CoverImage.EntityState = EntityState.Added;
                }

                var percentage = Utils.CalculatePercentage(index + 1, 1, _foundPdfFiles.Count);

                book.EntityState = EntityState.Added;

                if (_bookDomain.Exists(book.BookFile.FullPathAndFileNameWithExtension))
                {
                    _booksExisted++;
                    Logger.Log.Debug("Importer Skipped: " + book.Title + " already exists.");
                }
                else
                {
                    _bookDomain.AddBook(book);
                    Logger.Log.Debug("Imported: " + book.Title);

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
            Logger.Log.Debug(
                $"Found {_foundPdfFiles.Count()} files in {_source.SourceDirectoryUrl} with an extension of {FileTypes.PDF}");
            Logger.Log.Debug(
                $"Importing from {_source.SourceDirectoryUrl}: Subdirectories {includeSubDirectories}: Generate Covers: {generateCovers}");
            OnProgressStarted();
            Worker.RunWorkerAsync();
        }

        public void OnBookChanged(Book book, BookEventArgs.BookState bookState, int? progress)
        {
            BookChanged?.Invoke(this, new BookEventArgs {Book = book, State = bookState, Progress = progress});
        }

        private void OnProgressComplete()
        {
            ProgressComplete?.Invoke(this, null);
        }

        private void OnProgressStarted()
        {
            ProgressStarted?.Invoke(this, null);
        }

        private void OnProgressChange(ProgressWindowEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
    }
}