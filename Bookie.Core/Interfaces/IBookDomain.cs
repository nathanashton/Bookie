﻿namespace Bookie.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Model;

    public interface IBookDomain
    {
        IList<Book> GetAllBooks();
        Book GetBookByTitle(string title);
        void AddBook(params Book[] book);
        void UpdateBook(params Book[] book);
        void RemoveBook(params Book[] book);
        bool Exists(string filePath);
        Task<IList<Book>> GetAllAsync();
        IList<Book> GetNested();
        Book GetBookById(int id);
        Book SetUnchanged(Book book);
    }
}