namespace Bookie.Common
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public class SearchResult
    {
        public enum Search
        {
            Title,
            Isbn
        };

        public double Percentage { get; set; }

        public Book Book { get; set; }

        public List<Publisher> Publishers { get; set; }

        public List<Author> Authors { get; set; }

        public SearchResult()
        {
            Publishers = new List<Publisher>();
            Authors = new List<Author>();
        }
    }
}