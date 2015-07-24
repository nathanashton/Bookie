namespace Bookie.Views
{
    using System;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for NoteView.xaml
    /// </summary>
    public partial class NoteView
    {
        public NoteView()
        {
            InitializeComponent();
            ViewModel.NoteChanged += ViewModel_NoteChanged;
        }

        public NoteViewModel ViewModel => (NoteViewModel) Resources["ViewModel"];

        private void ViewModel_NoteChanged(object sender, EventArgs e)
        {
            Close();
        }
    }
}