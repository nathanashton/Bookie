namespace Bookie.UserControls
{
    using Bookie.ViewModels;
    /// <summary>
    ///     Interaction logic for PDFViewer.xaml
    /// </summary>
    public partial class PdfViewer
    {
        public PdfViewerViewModel ViewModel => (PdfViewerViewModel)Resources["ViewModel"];

        public PdfViewer()
        {
            InitializeComponent();
        }
    }
}