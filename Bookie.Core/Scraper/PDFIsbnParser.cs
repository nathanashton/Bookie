namespace Bookie.Core.Scraper
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using Common;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;

    internal class PdfIsbnParser
    {
        private string _isbn = string.Empty;

        public string Go(string url)
        {
            var text = new StringBuilder();
            try
            {
                using (var pdfReader = new PdfReader(url))
                {
                    // Loop through each page of the document
                    for (var page = 1; page <= 10; page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        try
                        {
                            var currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                            currentText =
                                Encoding.UTF8.GetString(
                                    Encoding.Convert(
                                        Encoding.Default,
                                        Encoding.UTF8,
                                        Encoding.Default.GetBytes(currentText)));
                            text.Append(currentText);
                        }
                        catch (ArgumentException)
                        {
                            Logger.Log.Error(string.Format("Can't parse PDF {0}, only images and no text.", url));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BookieException(ex.Message, ex);
            }

            var rFileIsbn = Regex.Match(text.ToString(), @"ISBN.*?([X\d\-_ .]{10,20})");
            if (!rFileIsbn.Success)
            {
                return null;
            }
            _isbn = rFileIsbn.Groups[1].ToString();
            _isbn = _isbn.Replace(".", string.Empty);
            _isbn = _isbn.Replace(" ", string.Empty);
            _isbn = _isbn.Replace("-", string.Empty);
            _isbn = _isbn.Replace("_", string.Empty);
            return _isbn;
        }
    }
}