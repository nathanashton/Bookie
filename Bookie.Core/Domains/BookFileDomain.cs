namespace Bookie.Core.Domains
{
    using Bookie.Common.Model;
    using Bookie.Core.Interfaces;
    using Bookie.Data.Interfaces;
    using Bookie.Data.Repositories;
    using System;
    using System.Collections.Generic;

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
            foreach (var b in bookfile)
            {
                b.CreatedDateTime = DateTime.Now;
                b.ModifiedDateTime = DateTime.Now;
            }
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