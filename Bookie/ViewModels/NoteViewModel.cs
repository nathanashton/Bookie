using System;

namespace Bookie.ViewModels
{
    using Bookie.Common;
    using Bookie.Common.Model;
    using Bookie.Core.Domains;
    using System.Windows;
    using System.Windows.Input;

    public class NoteViewModel : NotifyBase
    {
        public event EventHandler<EventArgs> NoteChanged;


        private Note _editing;

        public Window Window { get; set; }

        private string _noteText;

        private Book _book;

        private int? _pageNumber;

        private ICommand _addNoteCommand;

        private ICommand _saveNoteCommand;

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

        private ICommand _removeNoteCommand;

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
            get
            {
                return _noteText;
            }
            set
            {
                _noteText = value;
                NotifyPropertyChanged("NoteText");
            }
        }

        public void SaveNote()
        {
            _book = BookDomain.SetUnchanged(_book);

            _editing.NoteText = _noteText;

            _editing.EntityState = EntityState.Modified;
            new BookDomain().UpdateBook(_book);
            OnNoteChanged();

            Window.Close();
        }

        public void RemoveNote()
        {
            _book = BookDomain.SetUnchanged(_book);

            _editing.EntityState = EntityState.Deleted;
            new BookDomain().UpdateBook(_book);
            OnNoteChanged();
            Window.Close();
        }

        public NoteViewModel(Book book, int? pageNumber, Note note)
        {
            _editing = note;
            _book = book;
            _pageNumber = pageNumber;

            if (_editing != null)
            {
                NoteText = _editing.NoteText;
                _book = _editing.Book;
                _pageNumber = _editing.PageNumber;
            }
        }

        private void AddNote()
        {
            _book = BookDomain.SetUnchanged(_book);

            Note note = new Note { Book = _book, NoteText = _noteText, CreatedDateTime = DateTime.Now };

            if (_pageNumber != null)
            {
                note.PageNumber = _pageNumber;
            }

            note.EntityState = EntityState.Added;
            _book.Notes.Add(note);
            new BookDomain().UpdateBook(_book);

            OnNoteChanged();

            Window.Close();
        }

        public void OnNoteChanged()
        {
            if (NoteChanged != null)
            {
                NoteChanged(this,null);
            }
        }
    }
}