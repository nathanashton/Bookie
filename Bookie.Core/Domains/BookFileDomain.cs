namespace Bookie.Core.Domains
{
    using System.Collections.Generic;
    using Common.Model;
    using Data.Interfaces;
    using Data.Repositories;
    using Interfaces;

    public class BookFileDomain : IBookFileDomain
    {
        private readonly IBookFileRepository _bookFileRepository;

        public BookFileDomain()
        {
            _bookFileRepository = new BookFileRepository();
        }

        public IList<BookFile> GetAllBookFiles()
        {
            return _bookFileRepository.GetAll();
        }

        public BookFile GetBookFileByUrl(string bookFileUrl)
        {
            return _bookFileRepository.GetSingle(x => x.FullPathAndFileNameWithExtension.Equals(bookFileUrl));
        }

        public void AddBookFile(params BookFile[] bookfile)
        {
            _bookFileRepository.Add(bookfile);
        }

        public void UpdateBookFile(params BookFile[] bookfile)
        {
            _bookFileRepository.Update(bookfile);
        }

        public void RemoveBookFile(params BookFile[] bookfile)
        {
            _bookFileRepository.Remove(bookfile);
        }
    }
}