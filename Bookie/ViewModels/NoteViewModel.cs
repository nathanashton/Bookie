namespace Bookie.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Common;
    using Common.Model;
    using Core.Domains;

    public class NoteViewModel : NotifyBase
    {
        private readonly BookDomain _bookDomain;
        private ICommand _addNoteCommand;
        private Book _book;
        private Note _editing;
        private string _noteText;
        private int? _pageNumber;
        private ICommand _removeNoteCommand;
        private ICommand _saveNoteCommand;

        public NoteViewModel()
        {
            _bookDomain = new BookDomain();
        }

        public Note Editing
        {
            get { return _editing; }
            set
            {
                _editing = value;
                NotifyPropertyChanged("Editing");
            }
        }

        public Book Book
        {
            get { return _book; }
            set
            {
                _book = value;
                NotifyPropertyChanged("Book");
            }
        }

        public int? PageNumber
        {
            get { return _pageNumber; }
            set
            {
                _pageNumber = value;
                NotifyPropertyChanged("PageNumber");
            }
        }

        public Window Window { get; set; }

        public string NoteDate
        {
            get
            {
                if (_editing != null)
                {
                    return _editing.CreatedDateTime.ToString();
                }
                return "";
            }
        }

        public ICommand SaveNoteCommand
        {
            get
            {
                return _saveNoteCommand
                       ?? (_saveNoteCommand = new RelayCommand(p => SaveNote(), p => _editing != null));
            }
        }

        public ICommand RemoveNoteCommand
        {
            get
            {
                return _removeNoteCommand
                       ?? (_removeNoteCommand = new RelayCommand(p => RemoveNote(), p => _editing != null));
            }
        }

        public ICommand AddNoteCommand
        {
            get
            {
                return _addNoteCommand
                       ?? (_addNoteCommand = new RelayCommand(p => AddNote(), p => _editing == null));
            }
        }

        public string NoteText
        {
            get { return _noteText; }
            set
            {
                _noteText = value;
                NotifyPropertyChanged("NoteText");
            }
        }

        public void Set(Book book, Note note, int? pageNumber)
        {
            Book = book;
            Editing = note;
            PageNumber = pageNumber;

            if (Editing == null) return;
            NoteText = Editing.NoteText;
            Book = Editing.Book;
            PageNumber = Editing.PageNumber;
        }

        public event EventHandler<EventArgs> NoteChanged;

        public void SaveNote()
        {
            Book = BookDomain.SetUnchanged(_book);
            Editing.NoteText = _noteText;
            Editing.EntityState = EntityState.Modified;
            _bookDomain.UpdateBook(Book);
            OnNoteChanged();
        }

        public void RemoveNote()
        {
            Book = BookDomain.SetUnchanged(Book);
            Editing.EntityState = EntityState.Deleted;
            _bookDomain.UpdateBook(Book);
            OnNoteChanged();
        }

        private void AddNote()
        {
            Book = BookDomain.SetUnchanged(Book);
            var note = new Note {Book = Book, NoteText = _noteText, CreatedDateTime = DateTime.Now};
            if (PageNumber != null)
            {
                note.PageNumber = PageNumber;
            }
            note.EntityState = EntityState.Added;
            Book.Notes.Add(note);
            _bookDomain.UpdateBook(Book);
            OnNoteChanged();
        }

        public void OnNoteChanged()
        {
            NoteChanged?.Invoke(this, null);
        }
    }
}