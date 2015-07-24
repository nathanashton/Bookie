namespace Bookie.Views
{
    using MahApps.Metro.Controls;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for RemoveBookView.xaml
    /// </summary>
    public partial class RemoveBookView : MetroWindow
    {
        public RemoveBookView()
        {
            InitializeComponent();
            ViewModel.Window = this;
        }

        public RemoveBookViewModel ViewModel => (RemoveBookViewModel) Resources["ViewModel"];
    }
}