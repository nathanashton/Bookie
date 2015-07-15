namespace Bookie.Views
{
    using Bookie.ViewModels;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for ConfirmScrapeView.xaml
    /// </summary>
    public partial class ConfirmScrapeView : MetroWindow
    {
        public ConfirmScrapeViewModel _viewModel = new ConfirmScrapeViewModel();

        public ConfirmScrapeView()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}