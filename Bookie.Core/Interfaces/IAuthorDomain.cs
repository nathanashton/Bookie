namespace Bookie.Core.Interfaces
{
    using System.Collections.Generic;
    using Common.Model;

    public interface IAuthorDomain
    {
        IList<Author> GetAllAuthors();
        Author GetAuthorByName(string authorName);
        void AddAuthor(params Author[] author);
        void UpdateAuthor(params Author[] author);
        void RemoveAuthor(params Author[] author);
    }
}