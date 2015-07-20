using System.Collections.Generic;
using Bookie.Common.Model;

namespace Bookie.Core.Interfaces
{
    public interface IBookFileDomain
    {
        IList<BookFile> GetAllBookFiles();

        BookFile GetBookFileByUrl(string bookFileUrl);

        void AddBookFile(params BookFile[] bookfile);

        void UpdateBookFile(params BookFile[] bookfile);

        void RemoveBookFile(params BookFile[] bookfile);
    }
}