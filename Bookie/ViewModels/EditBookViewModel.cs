namespace Bookie.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Common;
    using Common.Model;
    using Core.Domains;

    public class EditBookViewModel : NotifyBase
    {
        private readonly BookDomain _bookDomain;
        private ICommand _cancelCommand;
        private ICommand _saveCommand;
        private Book _selectedBook;

        public EditBookViewModel()
        {
            _bookDomain = new BookDomain();
        }

        public Window Window { get; set; }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(p => Save(), p => true));
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand
                       ?? (_cancelCommand = new RelayCommand(p => Window.Close(), p => true));
            }
        }

        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                NotifyPropertyChanged("SelectedBook");
            }
        }

        public event EventHandler<BookEventArgs> BookChanged;

        private void Save()
        {
            SelectedBook.EntityState = EntityState.Modified;
            _bookDomain.UpdateBook(SelectedBook);
            OnBookChanged(SelectedBook, BookEventArgs.BookState.Updated, null);
            Window.Close();
        }

        public void OnBookChanged(Book book, BookEventArgs.BookState bookState, int? progress)
        {
            BookChanged?.Invoke(this, new BookEventArgs {Book = book, State = bookState, Progress = progress});
        }
    }
}