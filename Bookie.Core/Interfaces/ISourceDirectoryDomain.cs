namespace Bookie.Core.Interfaces
{
    using Bookie.Common.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISourceDirectoryDomain
    {
        IList<SourceDirectory> GetAllSourceDirectories();

        SourceDirectory GetSourceByUrlWithBooks(string sourceUrl);

        void AddSourceDirectory(params SourceDirectory[] sourceDirectory);

        void UpdateSourceDirectory(params SourceDirectory[] sourceDirectory);

        void RemoveSourceDirectory(params SourceDirectory[] sourceDirectory);

        bool Exists(string sourceUrl);

        SourceDirectory GetSourceDirectoryForBook(Book book);

        Task<IList<SourceDirectory>> GetAllSourceDirectoriesAsync();

    }
}