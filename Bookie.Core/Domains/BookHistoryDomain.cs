namespace Bookie.Core.Domains
{
    using System;
    using System.Collections.Generic;
    using Common.Model;
    using Data.Interfaces;
    using Data.Repositories;
    using Interfaces;

    public class BookHistoryDomain : IBookHistoryDomain
    {
        private readonly IBookHistoryRepository _bookHistoryRepository;

        public BookHistoryDomain()
        {
            _bookHistoryRepository = new BookHistoryRepository();
        }

        public IList<BookHistory> GetAllBookHistories()
        {
            return _bookHistoryRepository.GetAll();
        }

        public BookHistory GetBookHistoryForBook(Book book)
        {
            return _bookHistoryRepository.GetSingle(x => x.Book.Id.Equals(book.Id));
        }

        public void AddBookHistory(params BookHistory[] bookhistory)
        {
            foreach (var b in bookhistory)
            {
                b.CreatedDateTime = DateTime.Now;
                b.ModifiedDateTime = DateTime.Now;
            }
            _bookHistoryRepository.Add(bookhistory);
        }

        public void UpdateBookHistory(params BookHistory[] bookhistory)
        {
            _bookHistoryRepository.Update(bookhistory);
        }

        public void RemoveBookHstory(params BookHistory[] bookhistory)
        {
            _bookHistoryRepository.Remove(bookhistory);
        }
    }
}