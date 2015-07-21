namespace Bookie.UserControls
{
    using ViewModels;

    /// <summary>
    ///     Interaction logic for PDFViewer.xaml
    /// </summary>
    public partial class PdfViewer
    {
        public PdfViewer()
        {
            InitializeComponent();
        }

        public PdfViewerViewModel ViewModel => (PdfViewerViewModel) Resources["ViewModel"];
    }
}