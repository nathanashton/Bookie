using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Bookie.Common;
using Bookie.Common.Model;
using Bookie.Core.Domains;
using Bookie.Views;
using MoonPdfLib;

namespace Bookie.ViewModels
{
    public class PdfViewerViewModel : NotifyBase
    {
        //private void GotoPage_OnClick(object sender, RoutedEventArgs e)
        //{
        //    //if (gotopage.Text != "")
        //    //{
        //    //    PdfPanel.GotoPage(int.Parse(gotopage.Text));
        //    //}
        //}


        //private void ButtonAddBookMark(object sender, RoutedEventArgs e)
        //{
        //    foreach (var x in Book.BookMarks)
        //    {
        //        x.EntityState = EntityState.Unchanged;
        //    }
        //    foreach (var publisher in Book.Publishers)
        //    {
        //        publisher.EntityState = EntityState.Unchanged;
        //    }
        //    foreach (var author in Book.Authors)
        //    {
        //        author.EntityState = EntityState.Unchanged;
        //    }

        //    var b = new BookMark
        //    {
        //        Book = Book,
        //        BookMarkedPage = PdfPanel.GetCurrentPageNumber(),
        //        EntityState = EntityState.Added
        //    };
        //    Book.BookMarks.Add(b);
        //    new BookDomain().UpdateBook(Book);
        //    Refresh();
        //}


        private ICommand _addNoteCommand;
        private Book _book;
        private ICommand _bookMarkButtonCommand;
        private int _currentPage;
        private ICommand _deleteBookmarkCommand;
        private ICommand _editNoteCommand;
        private ICommand _gotoFirstPageCommand;
        private ICommand _gotoLastPageCommand;
        private ICommand _gotoNextPageCommand;
        private ICommand _gotoPreviousPageCommand;
        private Visibility _leftPane;
        private ICommand _leftPaneCommand;
        private ICommand _noteButtonCommand;
        private ICommand _pageContinuousCommand;
        private ICommand _pageSingleCommand;
        private MoonPdfPanel _pdfPanel;
        private Visibility _rightPane;
        private ICommand _rightPaneCommand;
        private BookMark _selectedBookMark;
        private Note _selectedNote;
        private ICommand _zoomHeightCommand;
        private ICommand _zoomInCommand;
        private ICommand _zoomOutCommand;
        private ICommand _zoomWidthCommand;
        //NoteLabel.Visibility = Visibility.Hidden;
        //BookMarkLabel.Visibility = Visibility.Hidden;


        public PdfViewerViewModel()
        {
            PdfPanel = new MoonPdfPanel();
            LeftPane = Visibility.Collapsed;
            RightPane = Visibility.Collapsed;
            SelectedBookMark = null;
            SelectedNote = null;
            PdfPanel.PageChangedEvent += moonPdfPanel_PageChangedEvent;
        }

        public MoonPdfPanel PdfPanel
        {
            get { return _pdfPanel; }
            set
            {
                _pdfPanel = value;
                NotifyPropertyChanged("PdfPanel");
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

        public Visibility LeftPane
        {
            get { return _leftPane; }
            set
            {
                _leftPane = value;
                NotifyPropertyChanged("LeftPane");
            }
        }

        public Visibility RightPane
        {
            get { return _rightPane; }
            set
            {
                _rightPane = value;
                NotifyPropertyChanged("RightPane");
            }
        }

        public BookMark SelectedBookMark
        {
            get { return _selectedBookMark; }
            set
            {
                _selectedBookMark = value;
                NotifyPropertyChanged("SelectedBookMark");
                if (_selectedBookMark != null)
                {
                    PdfPanel.GotoPage(_selectedBookMark.BookMarkedPage);
                }
            }
        }

        public Note SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                NotifyPropertyChanged("SelectedNote");
                if (_selectedNote != null && _selectedNote.PageNumber != null)
                {
                    PdfPanel.GotoPage(Convert.ToInt32(_selectedNote.PageNumber));
                }
            }
        }

        public ICommand DeleteBookMarkCommand
        {
            get
            {
                return _deleteBookmarkCommand
                       ?? (_deleteBookmarkCommand = new RelayCommand(DeleteBookMark, p => true));
            }
        }

        public ICommand EditNoteCommand
        {
            get
            {
                return _editNoteCommand
                       ?? (_editNoteCommand = new RelayCommand(EditNote, p => true));
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

        public ICommand GotoFirstPageCommand
        {
            get
            {
                return _gotoFirstPageCommand
                       ?? (_gotoFirstPageCommand = new RelayCommand(p => PdfPanel.GotoFirstPage(), p => Book != null));
            }
        }

        public ICommand GotoPreviousPageCommand
        {
            get
            {
                return _gotoPreviousPageCommand
                       ??
                       (_gotoPreviousPageCommand = new RelayCommand(p => PdfPanel.GotoPreviousPage(), p => Book != null));
            }
        }

        public ICommand GotoNextPageCommand
        {
            get
            {
                return _gotoNextPageCommand
                       ?? (_gotoNextPageCommand = new RelayCommand(p => PdfPanel.GotoNextPage(), p => Book != null));
            }
        }

        public ICommand GotoLastPageCommand
        {
            get
            {
                return _gotoLastPageCommand
                       ?? (_gotoLastPageCommand = new RelayCommand(p => PdfPanel.GotoLastPage(), p => Book != null));
            }
        }

        public ICommand ZoomWidthCommand
        {
            get
            {
                return _zoomWidthCommand
                       ?? (_zoomWidthCommand = new RelayCommand(p => PdfPanel.ZoomToWidth(), p => Book != null));
            }
        }

        public ICommand ZoomHeightCommand
        {
            get
            {
                return _zoomHeightCommand
                       ?? (_zoomHeightCommand = new RelayCommand(p => PdfPanel.ZoomToHeight(), p => Book != null));
            }
        }

        public ICommand PageSingleCommand
        {
            get
            {
                return _pageSingleCommand
                       ??
                       (_pageSingleCommand =
                           new RelayCommand(p => PdfPanel.PageRowDisplay = PageRowDisplayType.SinglePageRow,
                               p => Book != null));
            }
        }

        public ICommand PageContinuousCommand
        {
            get
            {
                return _pageContinuousCommand
                       ??
                       (_pageContinuousCommand =
                           new RelayCommand(p => PdfPanel.PageRowDisplay = PageRowDisplayType.ContinuousPageRows,
                               p => Book != null));
            }
        }

        public ICommand ZoomInCommand
        {
            get
            {
                return _zoomInCommand
                       ?? (_zoomInCommand = new RelayCommand(p => PdfPanel.ZoomIn(), p => Book != null));
            }
        }

        public ICommand ZoomOutCommand
        {
            get
            {
                return _zoomOutCommand
                       ?? (_zoomOutCommand = new RelayCommand(p => PdfPanel.ZoomOut(), p => Book != null));
            }
        }

        public ICommand AddNoteCommand
        {
            get
            {
                return _addNoteCommand
                       ?? (_addNoteCommand = new RelayCommand(p => AddNote(), p => Book != null));
            }
        }

        public ICommand BookMarkButtonCommand
        {
            get
            {
                return _bookMarkButtonCommand
                       ?? (_bookMarkButtonCommand = new RelayCommand(p => BookMarkClick(), p => Book != null));
            }
        }

        public ICommand NoteButtonCommand
        {
            get
            {
                return _noteButtonCommand
                       ?? (_noteButtonCommand = new RelayCommand(p => NoteClick(), p => Book != null));
            }
        }

        private void LeftPaneToggle()
        {
            LeftPane = LeftPane == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void RightPaneToggle()
        {
            RightPane = RightPane == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void moonPdfPanel_PageChangedEvent(object sender, EventArgs e)
        {
            _currentPage = PdfPanel.GetCurrentPageNumber();
            //   pageCount.Content = PdfPanel.GetCurrentPageNumber() + "/" + PdfPanel.TotalPages;
            var b = Book.Notes.FirstOrDefault(x => x.PageNumber == _currentPage);
            if (b != null)
            {
                SelectedNote = b;
                // NoteLabel.Visibility = Visibility.Visible;
            }
            else
            {
                SelectedNote = null;
                //     NoteLabel.Visibility = Visibility.Hidden;
            }

            var c = Book.BookMarks.FirstOrDefault(x => x.BookMarkedPage == _currentPage);
            if (c != null)
            {
                SelectedBookMark = c;
                // BookMarkLabel.Visibility = Visibility.Visible;
            }
            else
            {
                SelectedBookMark = null;
                //  BookMarkLabel.Visibility = Visibility.Hidden;
            }
        }

        public void Refresh()
        {
            Book = new BookDomain().GetBookById(Book.Id);
        }

        public void OpenPdf(string url)
        {
            PdfPanel.OpenFile(url);
            //pageCount.Content = PdfPanel.GetCurrentPageNumber() + "/" + PdfPanel.TotalPages;
        }

        private void AddNote()
        {
            var view = new NoteView();
            view.ViewModel.Set(Book, null, PdfPanel.GetCurrentPageNumber());
            view.ViewModel.NoteChanged += _viewModel_NoteChanged;
            view.ShowDialog();
        }

        private void EditNote(object parameter)
        {
            var view = new NoteView();
            view.ViewModel.Set(Book, (Note) parameter, PdfPanel.GetCurrentPageNumber());
            view.ViewModel.NoteChanged += _viewModel_NoteChanged;
            view.ShowDialog();
        }

        private void DeleteBookMark(object parameter)
        {
            foreach (var x in Book.BookMarks)
            {
                x.EntityState = EntityState.Unchanged;
            }
            foreach (var publisher in Book.Publishers)
            {
                publisher.EntityState = EntityState.Unchanged;
            }
            foreach (var author in Book.Authors)
            {
                author.EntityState = EntityState.Unchanged;
            }

            var bb = (BookMark) parameter;
            bb.EntityState = EntityState.Deleted;

            Book.BookMarks.ToList().RemoveAll(x => x.Id == bb.Id);
            new BookDomain().UpdateBook(Book);
            Refresh();
        }

        private void _viewModel_NoteChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void BookMarkClick()
        {
            if (LeftPane == Visibility.Collapsed)
            {
                LeftPane = Visibility.Visible;
            }
        }

        private void NoteClick()
        {
            if (RightPane == Visibility.Collapsed)
            {
                RightPane = Visibility.Visible;
            }
            EditNote(SelectedNote);
        }
    }
}