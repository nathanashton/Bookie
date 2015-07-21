namespace Bookie.Core.Interfaces
{
    using System.Collections.Generic;
    using Common.Model;

    public interface IBookMarkDomain
    {
        IList<BookMark> GetAllBookMarks();
        IList<BookMark> GetBookMarksForBook(Book book);
        void AddBookMark(params BookMark[] bookmark);
        void UpdateBookMark(params BookMark[] bookmark);
        void RemoveBookMark(params BookMark[] bookmark);
    }
}