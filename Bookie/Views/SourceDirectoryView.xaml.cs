namespace Bookie.Views
{
    using MahApps.Metro.Controls;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for SourceDirectoryView.xaml
    /// </summary>
    public partial class SourceDirectoryView : MetroWindow
    {
        private readonly SourceDirectoryViewModel _viewModel;

        public SourceDirectoryView(MainViewModel mainWindowViewModel)
        {
            InitializeComponent();

            _viewModel = new SourceDirectoryViewModel(mainWindowViewModel);
            DataContext = _viewModel;
        }
    }
}