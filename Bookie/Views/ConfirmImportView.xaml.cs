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
    using Bookie.ViewModels;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for ConfirmImportView.xaml
    /// </summary>
    public partial class ConfirmImportView : MetroWindow
    {

        private ConfirmImportViewModel _viewModel = new ConfirmImportViewModel();

        public ConfirmImportView()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
