namespace Bookie.ViewModels
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;
    using Common;
    using Common.Model;
    using Core.Domains;

    public class RemoveBookViewModel : NotifyBase
    {
        private readonly BookDomain _bookDomain;
        private ICommand _cancelCommand;
        private bool _deleteFile;
        private bool _exclude;
        private ICommand _removeCommand;
        private Book _selectedBook;

        public RemoveBookViewModel()
        {
            _bookDomain = new BookDomain();
        }

        public bool DeleteFile
        {
            get { return _deleteFile; }
            set
            {
                _deleteFile = value;
                NotifyPropertyChanged("DeleteFile");
            }
        }

        public bool Exclude
        {
            get { return _exclude; }
            set
            {
                _exclude = value;
                NotifyPropertyChanged("Exclude");
            }
        }

        public Window Window { get; set; }

        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                NotifyPropertyChanged("SelectedBook");
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand
                       ?? (_removeCommand = new RelayCommand(p => Remove(), p => true));
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand
                       ?? (_cancelCommand = new RelayCommand(p => Cancel(), p => true));
            }
        }

        public event EventHandler<BookEventArgs> BookChanged;

        private void Remove()
        {
            var path = SelectedBook.BookFile.FullPathAndFileNameWithExtension;

            _bookDomain.RemoveBook(SelectedBook);
            if (DeleteFile)
            {
                File.Delete(path);
            }
            if (Exclude)
            {
                var ex = new Excluded();
                ex.Url = path;
                ex.EntityState = EntityState.Added;
                new ExcludedDomain().AddExcluded(ex);
            }

            OnBookChanged(SelectedBook, BookEventArgs.BookState.Removed, null);
            Window.Close();
        }

        private void Cancel()
        {
            Window.Close();
        }

        public void OnBookChanged(Book book, BookEventArgs.BookState bookState, int? progress)
        {
            BookChanged?.Invoke(this, new BookEventArgs {Book = book, State = bookState, Progress = progress});
        }
    }
}