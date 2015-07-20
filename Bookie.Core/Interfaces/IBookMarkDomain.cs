using System.Collections.Generic;
using Bookie.Common.Model;

namespace Bookie.Core.Interfaces
{
    public interface IBookMarkDomain
    {
        IList<BookMark> GetAllBookMarks();

        IList<BookMark> GetBookMarksForBook(Book book);

        void AddBookMark(params BookMark[] bookmark);

        void UpdateBookMark(params BookMark[] bookmark);

        void RemoveBookMark(params BookMark[] bookmark);
    }
}