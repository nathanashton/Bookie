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
    /// Interaction logic for ConfirmScrapeView.xaml
    /// </summary>
    public partial class ConfirmScrapeView : MetroWindow
    {
        private ConfirmScrapeViewModel _viewModel = new ConfirmScrapeViewModel();


        public ConfirmScrapeView()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }
    }
}
