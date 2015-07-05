namespace Bookie.Views
{
    using Bookie.ViewModels;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for SourceDirectoryView.xaml
    /// </summary>
    public partial class SourceDirectoryView : MetroWindow
    {
        private SourceDirectoryViewModel _viewModel;

        public SourceDirectoryView(MainViewModel mainWindowViewModel)
        {
            InitializeComponent();

            _viewModel = new SourceDirectoryViewModel(mainWindowViewModel);
            DataContext = _viewModel;
        }
    }
}