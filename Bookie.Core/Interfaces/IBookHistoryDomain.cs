namespace Bookie.Core.Interfaces
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public interface IBookHistoryDomain
    {
        IList<BookHistory> GetAllBookHistories();

        BookHistory GetBookHistoryForBook(Book book);

        void AddBookHistory(params BookHistory[] bookhistory);

        void UpdateBookHistory(params BookHistory[] bookhistory);

        void RemoveBookHstory(params BookHistory[] bookhistory);
    }
}