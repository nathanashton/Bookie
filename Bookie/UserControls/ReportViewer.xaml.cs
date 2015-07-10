using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Bookie.UserControls
{
    using Bookie.Common.Model;
    using Bookie.Core.Domains;
    using Microsoft.Reporting.WinForms;
    using System.Data;

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

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("Abstract", typeof(string)));
            foreach (var book in books)
            {
                DataRow dr = dt.NewRow();
                dr["Title"] = book.Title;
                dr["Abstract"] = book.Abstract;
                dt.Rows.Add(dr);
            }

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1"; // Name of the DataSet we set in .rdlc
            reportDataSource.Value = dt;
            reportViewer.LocalReport.ReportPath = "AllBooks.rdlc"; // Path of the rdlc file

            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.RefreshReport();
        }

        private void reportViewer_RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {
        }
    }
}