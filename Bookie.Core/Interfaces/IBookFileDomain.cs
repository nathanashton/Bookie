namespace Bookie.Core.Interfaces
{
    using System.Collections.Generic;
    using Common.Model;

    public interface IBookFileDomain
    {
        IList<BookFile> GetAllBookFiles();
        BookFile GetBookFileByUrl(string bookFileUrl);
        void AddBookFile(params BookFile[] bookfile);
        void UpdateBookFile(params BookFile[] bookfile);
        void RemoveBookFile(params BookFile[] bookfile);
    }
}