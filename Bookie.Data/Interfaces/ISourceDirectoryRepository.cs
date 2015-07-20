using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bookie.Common.Model;

namespace Bookie.Data.Interfaces
{
    public interface ISourceDirectoryRepository : IGenericDataRepository<SourceDirectory>
    {
        bool Exists(string sourceUrl);

        Task<IList<SourceDirectory>> GetAllAsync(params Expression<Func<SourceDirectory, object>>[] navigationProperties);
    }
}