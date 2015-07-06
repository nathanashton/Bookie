using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookie.ViewModels
{
    using System.Windows;
    using System.Windows.Input;

    using Bookie.Common;
    using Bookie.Common.Model;
    using Bookie.Core.Domains;

    public class NoteViewModel : NotifyBase
    {
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
                       ?? (_addNoteCommand = new RelayCommand(p => AddNote(), p=> _editing == null));
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
            _book.SetUnchanged();

            _editing.NoteText = _noteText;

            _editing.EntityState = EntityState.Modified;
            new BookDomain().UpdateBook(_book);
            Window.Close();
        }

        public void RemoveNote()
        {
            _book.SetUnchanged();

          

            _editing.EntityState = EntityState.Deleted;
            new BookDomain().UpdateBook(_book);
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
            _book.SetUnchanged();

            Note note = new Note { Book = _book, NoteText = _noteText, CreatedDateTime = DateTime.Now};

            if (_pageNumber != null)
            {
                note.PageNumber = _pageNumber;
            }

            note.EntityState = EntityState.Added;
            _book.Notes.Add(note);
            new BookDomain().UpdateBook(_book);
            Window.Close();
        }

        
    }
}
