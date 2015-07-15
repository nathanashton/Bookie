using System.Windows;

namespace Bookie.Views
{
    using Bookie.ViewModels;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for ConfirmImportView.xaml
    /// </summary>
    public partial class ConfirmImportView : MetroWindow
    {
        public ConfirmImportViewModel _viewModel = new ConfirmImportViewModel();

        public ConfirmImportView()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}