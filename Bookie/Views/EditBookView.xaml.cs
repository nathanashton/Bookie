namespace Bookie.Views
{
    using MahApps.Metro.Controls;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for EditBookView.xaml
    /// </summary>
    public partial class EditBookView : MetroWindow
    {
        public EditBookView()
        {
            InitializeComponent();
            ViewModel.Window = this;
        }

        public EditBookViewModel ViewModel => (EditBookViewModel) Resources["ViewModel"];
    }
}