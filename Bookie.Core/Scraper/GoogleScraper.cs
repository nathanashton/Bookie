namespace Bookie.Core.Scraper
{
    using Bookie.Common;
    using Bookie.Common.Model;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Xml;

    public class GoogleScraper : NotifyBase, IBookScraper
    {
        private static DateTime lastTimeIWasCalled = DateTime.Now;

        public SearchResult.Search SearchBy { get; set; }

        public object SearchQuery { get; set; }

        public ObservableCollection<SearchResult> SearchBooks(object searchQuery)
        {
            var searchResults = new ObservableCollection<SearchResult>();

            SearchQuery = searchQuery;

            if (searchQuery == null)
            {
                throw new ArgumentNullException("searchQuery");
            }

            if (DateTime.Now.Subtract(lastTimeIWasCalled).Seconds < 1)
            {
                System.Threading.Thread.Sleep(1000);
            }

            lastTimeIWasCalled = DateTime.Now;

            var retrieveUrl = GetUrlForSearch();

            var xdcDocument = new XmlDocument();

            xdcDocument.Load(retrieveUrl);

            var ns = new XmlNamespaceManager(xdcDocument.NameTable);
            ns.AddNamespace("openSearch", "http://a9.com/-/spec/opensearchrss/1.0/");
            ns.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            ns.AddNamespace("dc", "http://purl.org/dc/terms");

            var xelRoot = xdcDocument.DocumentElement;
            var xnlNodes = xelRoot.SelectNodes("//atom:entry", ns);

            foreach (XmlNode xndNode in xnlNodes)
            {
                var searchResult = new SearchResult();
                var book = new Book();

                var xmlTitle = xndNode["title"];
                if (xmlTitle != null)
                {
                    book.Title = xmlTitle.InnerText;
                }
                var xmlDatePublished = xndNode["dc:date"];
                if (xmlDatePublished != null)
                {
                    var dp = xmlDatePublished.InnerText;
                    if (dp.Length == 4)
                    {
                        var dt = Convert.ToDateTime(String.Format("01/01/,{0}", dp));
                        book.DatePublished = dt;
                    }
                    else
                    {
                        DateTime dt;
                        if (DateTime.TryParse(dp, out dt))
                        {
                            book.DatePublished = dt;
                        }
                    }
                }

                var xmlDescription = xndNode["dc:description"];
                if (xmlDescription != null)
                {
                   // Remove ASCII Control Characters
                    book.Abstract = xmlDescription.InnerText.Replace("&#39;", "'");
                    book.Abstract = xmlDescription.InnerText.Replace("&quot;", String.Empty);
                }

                var formatNodes = xndNode.SelectNodes("dc:format", ns);
                if (formatNodes != null)
                {
                    foreach (XmlNode node in formatNodes)
                    {
                        if (node.InnerText.Contains("pages"))
                        {
                            var resultString = Regex.Match(node.InnerText, @"\d+").Value;
                            book.Pages = Convert.ToInt32(resultString);
                        }
                    }
                }

                var identifierNodes = xndNode.SelectNodes("dc:identifier", ns);
                if (identifierNodes != null)
                {
                    foreach (XmlNode node in identifierNodes)
                    {
                        if (node.InnerText.Length == 18) // 13 digit ISBN Node
                        {
                            book.Isbn = Regex.Match(node.InnerText, @"\d+").Value;
                        }
                    }
                }

                var xmlPublisher = xndNode["dc:publisher"];
                if (xmlPublisher != null)
                {
                    searchResult.Publishers.Add(new Publisher() { Name = xmlPublisher.InnerText });
                }

                var xmlAuthor = xndNode["dc:creator"];
                if (xmlAuthor != null)
                {
                    var words = xmlAuthor.InnerText.Split(' ');
                    if (words.Length == 1)
                    {
                        searchResult.Authors.Add(new Author() { LastName = words[0] });
                    }
                    else
                    {
                        searchResult.Authors.Add(new Author()
                        {
                            FirstName = words[0],
                            LastName = words[1]
                        });
                    }
                }

                book.Scraped = true;
                searchResult.Book = book;
                // searchResult.Percentage = MatchStrings.Compute(searchQuery.ToString(), book.Title)*100;
                searchResults.Add(searchResult);
            }
            var s = searchResults.OrderByDescending(x => x.Percentage);
            return new ObservableCollection<SearchResult>(s);
        }

        private string GetUrlForSearch()
        {
            SearchQuery = WebUtility.UrlEncode(SearchQuery.ToString());

            return "http://books.google.com/books/feeds/volumes?q=isbn:" + SearchQuery + "&max-results=20&start-index=1&min-viewability=none";
        }
    }
}