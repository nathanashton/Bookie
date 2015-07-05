namespace Bookie.Core.Domains
{
    using Bookie.Common.Model;
    using Bookie.Core.Interfaces;
    using Bookie.Data.Interfaces;
    using Bookie.Data.Repositories;
    using System;
    using System.Linq;
    using System.Collections.Generic;

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

        public IList<BookMark >GetBookMarksForBook(Book book)
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