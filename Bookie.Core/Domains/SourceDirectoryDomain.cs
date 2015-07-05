namespace Bookie.Core.Domains
{
    using Bookie.Common.Model;
    using Bookie.Core.Interfaces;
    using Bookie.Data.Interfaces;
    using Bookie.Data.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SourceDirectoryDomain : ISourceDirectoryDomain
    {
        private readonly ISourceDirectoryRepository _sourceRepository;

        public SourceDirectoryDomain()
        {
            _sourceRepository = new SourceDirectoryRepository();
        }

        public IList<SourceDirectory> GetAllSourceDirectories()
        {
            return _sourceRepository.GetAll(x => x.Books);
        }

        public async Task<IList<SourceDirectory>> GetAllSourceDirectoriesAsync()
        {
            return await _sourceRepository.GetAllAsync(x => x.Books);
        }

        public SourceDirectory GetSourceByUrlWithBooks(string sourceUrl)
        {
            var p = _sourceRepository.GetSingle(d => d.SourceDirectoryUrl.Equals(sourceUrl), d => d.Books.Select(r => r.BookFile));
            return p;
        }

        public void AddSourceDirectory(params SourceDirectory[] sourceDirectory)
        {
            if (Exists(sourceDirectory[0].SourceDirectoryUrl))
            {
                return;
            }
            foreach (var b in sourceDirectory)
            {
                b.CreatedDateTime = DateTime.Now;
                b.ModifiedDateTime = DateTime.Now;
            }
            _sourceRepository.Add(sourceDirectory);
        }

        public void UpdateSourceDirectory(params SourceDirectory[] sourceDirectory)
        {
            _sourceRepository.Update(sourceDirectory);
        }

        public void RemoveSourceDirectory(params SourceDirectory[] sourceDirectory)
        {
            foreach (var sourece in sourceDirectory)
            {
                sourece.EntityState = EntityState.Deleted;
                _sourceRepository.Remove(sourceDirectory);
            }
        }

        public bool Exists(string sourceUrl)
        {
            return _sourceRepository.Exists(sourceUrl);
        }

        public SourceDirectory GetSourceDirectoryForBook(Book book)
        {
            return _sourceRepository.GetSingle(x => x.Id == book.SourceDirectory.Id);
        }
    }
}