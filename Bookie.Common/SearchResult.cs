using System.Collections.Generic;
using Bookie.Common.Model;

namespace Bookie.Common
{
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