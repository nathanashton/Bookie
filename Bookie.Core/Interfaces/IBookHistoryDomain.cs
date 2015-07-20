using System.Collections.Generic;
using Bookie.Common.Model;

namespace Bookie.Core.Interfaces
{
    public interface IBookHistoryDomain
    {
        IList<BookHistory> GetAllBookHistories();

        BookHistory GetBookHistoryForBook(Book book);

        void AddBookHistory(params BookHistory[] bookhistory);

        void UpdateBookHistory(params BookHistory[] bookhistory);

        void RemoveBookHstory(params BookHistory[] bookhistory);
    }
}