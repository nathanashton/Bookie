/***************************************************************************************\
    eLibrary: ebooks organizer and manager utitlity
    Copyright (C) 2008-2009 Amir Shaked <admin.ebooklibrary@gmail.com>
	http://www.openelibrary.org

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
\***************************************************************************************/

namespace Bookie.Core.Scraper
{
    using Bookie.Common;
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web;

    public class IsbnGuesser
    {
        protected string SimpleBrowseToPage(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            WebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

            }
            catch (WebException)
            {
                Logger.Log.Error("No internet connection while scraping");
                return String.Empty;
            }
            var reader = new StreamReader(response.GetResponseStream());
            var documentText = reader.ReadToEnd();
            response.Close();
            return documentText;
        }

        public static String Isbn13to10(String isbn13)
        {
            if (String.IsNullOrEmpty(isbn13))
                throw new ArgumentNullException("isbn13");
            isbn13 = isbn13.Replace("-", "").Replace(" ", "");
            if (isbn13.Length != 13)
                throw new ArgumentException("The ISBN doesn't contain 13 characters.", "isbn13");

            String isbn10 = isbn13.Substring(3, 9);
            int checksum = 0;
            int weight = 10;

            foreach (Char c in isbn10)
            {
                checksum += (int)Char.GetNumericValue(c) * weight;
                weight--;
            }

            checksum = 11 - (checksum % 11);
            if (checksum == 10)
                isbn10 += "X";
            else if (checksum == 11)
                isbn10 += "0";
            else
                isbn10 += checksum;

            return isbn10;
        }

        public string GuessBookIsbn(string fullPath)
        {
            string isbn;
            var fileName = Path.GetFileName(fullPath);
            if (String.IsNullOrEmpty(fileName))
            {
                return null;
            }

            // Step 1: Check if the book path has the ISBN
            var strMatch = @"[\d]+X?";
            if (Regex.IsMatch(fileName, strMatch))
            {
                for (var m = Regex.Match(fileName, strMatch); m.Success; m = m.NextMatch())
                {
                    isbn = m.ToString();
                    if (isbn.Length == 10 || isbn.Length == 13)
                        return isbn;
                }
            }

            strMatch = @"[\d\.\-_ ]+X?";
            if (Regex.IsMatch(fileName, strMatch))
            {
                for (var m = Regex.Match(fileName, strMatch); m.Success; m = m.NextMatch())
                {
                    isbn = m.ToString();
                    if (isbn.Length < 10) continue;
                    isbn = isbn.Replace(".", string.Empty);
                    isbn = isbn.Replace(" ", string.Empty);
                    isbn = isbn.Replace("_", string.Empty);
                    if (isbn.Length > 13) continue;

                    if (isbn.Length == 10 || isbn.Length == 13)
                        return isbn;
                }
            }

            // Step 2:
            // if book is PDF then read the ext and search for a line
            var extension = Path.GetExtension(fullPath);
            if (extension != null && extension.ToUpper().Equals(".PDF"))
            {
                var pdfTextParser = new PdfIsbnParser();
                try
                {
                    var foundIsbn = pdfTextParser.Go(fullPath);
                    if (foundIsbn != string.Empty) return foundIsbn;
                }
                catch (BookieException ex)
                {
                    Logger.Log.Error("Unable to parse PDF", ex);
                }
            }

            // Step 3:
            // Use the path as a book title and search for it
            var bookTitle = fileName.Replace(".", " ");
            bookTitle = bookTitle.Replace("_", " ");
            bookTitle = bookTitle.Replace("-", " ");
            var searchUrl = "http://www.google.com/search?hl=en&q=" + HttpUtility.UrlEncode(bookTitle);
            var documentText = SimpleBrowseToPage(searchUrl);
            var r1 = Regex.Match(documentText,
                                   @"www.amazon.com/.*?/(\d{9}X|\d{10,13})");
            if (r1.Success)
            {
                isbn = r1.Groups[1].ToString();
                return isbn;
            }

            searchUrl = "http://www.google.com/search?hl=en&q=%22" + HttpUtility.UrlEncode(bookTitle);
            searchUrl += "%22+amazon";
            documentText = SimpleBrowseToPage(searchUrl);
            var r2 = Regex.Match(documentText,
                                   @"www.amazon.com/.*?/(\d{9}X|\d{10,13})");
            if (!r2.Success)
            {
                return string.Empty;
            }
            isbn = r2.Groups[1].ToString();
            return isbn;
        }
    }
}