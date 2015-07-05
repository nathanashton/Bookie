namespace Bookie.Core
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public class AuthorTreeView
    {
        public Author Author { get; set; }

        public List<Book> Books { get; set; }

        public AuthorTreeView()
        {
            Books = new List<Book>();
        }
    }
}