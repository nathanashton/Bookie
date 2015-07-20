using System.Collections.Generic;
using Bookie.Common.Model;

namespace Bookie.Core.Interfaces
{
    public interface IAuthorDomain
    {
        IList<Author> GetAllAuthors();

        Author GetAuthorByName(string authorName);

        void AddAuthor(params Author[] author);

        void UpdateAuthor(params Author[] author);

        void RemoveAuthor(params Author[] author);
    }
}