namespace Bookie.Common
{
    using System.Collections.Generic;
    using Model;

    public class SearchResult
    {
        public enum Search
        {
            Title,
            Isbn
        };

        public SearchResult()
        {
            Publishers = new List<Publisher>();
            Authors = new List<Author>();
        }

        public double Percentage { get; set; }
        public Book Book { get; set; }
        public List<Publisher> Publishers { get; set; }
        public List<Author> Authors { get; set; }
    }
}