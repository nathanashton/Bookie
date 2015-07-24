namespace Bookie.Views
{
    using MahApps.Metro.Controls;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for ProgressView.xaml
    /// </summary>
    public partial class ProgressView : MetroWindow
    {
        public ProgressView()
        {
            InitializeComponent();
            ViewModel.Window = this;
        }

        public ProgressViewModel ViewModel => (ProgressViewModel) Resources["ViewModel"];
    }
}