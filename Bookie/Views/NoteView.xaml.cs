using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bookie.Views
{
    using Bookie.Common.Model;
    using Bookie.ViewModels;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for NoteView.xaml
    /// </summary>
    public partial class NoteView : MetroWindow
    {

        public NoteViewModel _viewModel;

        public NoteView(Book book, int? pageNumber, Note note)
        {
            InitializeComponent();
            _viewModel = new NoteViewModel(book, pageNumber, note);
            this.DataContext = _viewModel;
            _viewModel.Window = this;
        }
    }
}
