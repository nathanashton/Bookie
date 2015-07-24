namespace Bookie.UserControls
{
    using System.Windows;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for PDFViewer.xaml
    /// </summary>
    public partial class PdfViewer
    {
        public PdfViewer()
        {
            InitializeComponent();
            var w = Application.Current.MainWindow;
            ViewModel.Window = w;
        }

        public PdfViewerViewModel ViewModel => (PdfViewerViewModel) Resources["ViewModel"];
    }
}