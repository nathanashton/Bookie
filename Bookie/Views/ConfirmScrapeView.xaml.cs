namespace Bookie.Views
{
    using System.Windows;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for ConfirmScrapeView.xaml
    /// </summary>
    public partial class ConfirmScrapeView
    {
        public ConfirmScrapeViewModel _viewModel = new ConfirmScrapeViewModel();

        public ConfirmScrapeView()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}