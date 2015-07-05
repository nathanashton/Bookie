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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookie.UserControls
{
    using Bookie.Common.Model;
    using Bookie.Core.Domains;

    using Microsoft.Reporting.WinForms;

    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : UserControl
    {
        public ReportViewer()
        {
            InitializeComponent();
        }

        private void reportViewer_Load(object sender, EventArgs e)
        {
            List<Book> books = new BookDomain().GetAllBooks().ToList();
          //  ReportDataSource reportDataSource = new ReportDataSource();
           // reportDataSource.Name = "BokkieData"; // Name of the DataSet we set in .rdlc
            reportViewer.LocalReport.ReportPath = "Report1.rdlc"; // Path of the rdlc file

          //  reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("BokkieData", books));
            reportViewer.RefreshReport();
        }

        private void reportViewer_RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {

        }
    }
}
