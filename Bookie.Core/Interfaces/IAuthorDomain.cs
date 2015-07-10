namespace Bookie.Core.Interfaces
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public interface IAuthorDomain
    {
        IList<Author> GetAllAuthors();

        Author GetAuthorByName(string authorName);

        void AddAuthor(params Author[] author);

        void UpdateAuthor(params Author[] author);

        void RemoveAuthor(params Author[] author);

    }
}