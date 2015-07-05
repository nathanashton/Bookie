namespace Bookie.Core.Interfaces
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public interface IBookFileDomain
    {
        IList<BookFile> GetAllBookFiles();

        BookFile GetBookFileByUrl(string bookFileUrl);

        void AddBookFile(params BookFile[] bookfile);

        void UpdateBookFile(params BookFile[] bookfile);

        void RemoveBookFile(params BookFile[] bookfile);
    }
}