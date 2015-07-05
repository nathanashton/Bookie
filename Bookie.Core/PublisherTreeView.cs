namespace Bookie.Core
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public class PublisherTreeView
    {
        public Publisher Publisher { get; set; }

        public List<Book> Books { get; set; }

        public PublisherTreeView()
        {
            Books = new List<Book>();
        }
    }
}