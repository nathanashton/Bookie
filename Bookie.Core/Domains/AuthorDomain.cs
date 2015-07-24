namespace Bookie.Core.Domains
{
    using System.Collections.Generic;
    using Common.Model;
    using Data.Interfaces;
    using Data.Repositories;
    using Interfaces;

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
    }
}