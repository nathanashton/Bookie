using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Bookie.ViewModels;
using MahApps.Metro.Controls;

namespace Bookie.Views
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow
    {
        private readonly LogViewModel _viewModel;

        public LogWindow(LogViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            datePicker.Loaded += delegate
            {
                var textBox1 = (TextBox)datePicker.Template.FindName("PART_TextBox", datePicker);
                textBox1.Background = datePicker.Background;
                textBox1.BorderThickness = new Thickness(0, 0, 0, 0);
                textBox1.BorderBrush = Brushes.Transparent;
            };
            _viewModel.RefreshLog();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RefreshLog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _viewModel.FilterDate = null;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}