using System;
using System.Collections.Generic;
using Bookie.Common.Model;
using Bookie.Core.Interfaces;
using Bookie.Data.Interfaces;
using Bookie.Data.Repositories;

namespace Bookie.Core.Domains
{
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
    }
}