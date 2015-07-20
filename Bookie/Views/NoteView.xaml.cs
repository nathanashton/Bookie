namespace Bookie.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for NoteView.xaml
    /// </summary>
    public partial class NoteView
    {
        public NoteViewModel ViewModel => (NoteViewModel)Resources["ViewModel"];

        public NoteView()
        {
            InitializeComponent();
            ViewModel.NoteChanged += ViewModel_NoteChanged;
        }

        private void ViewModel_NoteChanged(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}