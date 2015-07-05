namespace Bookie.Core.Domains
{
    using Bookie.Common.Model;
    using Bookie.Core.Interfaces;
    using Bookie.Data.Interfaces;
    using Bookie.Data.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AuthorDomain : IAuthorDomain
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorDomain()
        {
            _authorRepository = new AuthorRepository();
        }

        public IList<Author> GetAllAuthors()
        {
            return _authorRepository.GetAll();
        }

        public Author GetAuthorByName(string authorName)
        {
            return _authorRepository.GetSingle(x => x.FullName.Equals(authorName));
        }

        public void AddAuthor(params Author[] author)
        {
            foreach (var b in author)
            {
                b.CreatedDateTime = DateTime.Now;
                b.ModifiedDateTime = DateTime.Now;
            }
            _authorRepository.Add(author);
        }

        public void UpdateAuthor(params Author[] author)
        {
            _authorRepository.Update(author);
        }

        public void RemoveAuthor(params Author[] author)
        {
            _authorRepository.Remove(author);
        }

        public List<AuthorTreeView> GetAuthorTreeView()
        {
            var allBooks = new BookDomain().GetAllBooks().ToList();

            List<AuthorTreeView> Authors = new List<AuthorTreeView>();

            var allAuthors = _authorRepository.GetAll(x => x.Book).ToList();

            HashSet<string> elements = new HashSet<string>(); // Type of property
            allAuthors.RemoveAll(i => !elements.Add(i.FullName));

            foreach (var author in allAuthors)
            {
                AuthorTreeView tree = new AuthorTreeView();
                tree.Author = author;
                foreach (var book in allBooks)
                {
                    foreach (var auth in book.Authors)
                    {
                        if (auth.FullName == author.FullName)
                        {
                            tree.Books.Add(book);
                        }
                    }
                }
                Authors.Add(tree);
            }
            return Authors;
        }
    }
}