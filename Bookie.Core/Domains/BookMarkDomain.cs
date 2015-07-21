namespace Bookie.Core.Domains
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Model;
    using Data.Interfaces;
    using Data.Repositories;
    using Interfaces;

    public class BookMarkDomain : IBookMarkDomain
    {
        private readonly IBookMarkRepository _bookMarkRepository;

        public BookMarkDomain()
        {
            _bookMarkRepository = new BookMarkRepository();
        }

        public IList<BookMark> GetAllBookMarks()
        {
            return _bookMarkRepository.GetAll(x => x.Book);
        }

        public IList<BookMark> GetBookMarksForBook(Book book)
        {
            return _bookMarkRepository.GetAll(x => x.Book).Where(y => y.Book.Id == book.Id).ToList();
        }

        public void AddBookMark(params BookMark[] bookmark)
        {
            _bookMarkRepository.Add(bookmark);
        }

        public void UpdateBookMark(params BookMark[] bookmark)
        {
            _bookMarkRepository.Update(bookmark);
        }

        public void RemoveBookMark(params BookMark[] bookmark)
        {
            throw new NotImplementedException();
        }
    }
}