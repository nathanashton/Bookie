namespace Bookie.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Common.Model;
    using Interfaces;

    public class SourceDirectoryRepository : GenericDataRepository<SourceDirectory>, ISourceDirectoryRepository
    {
        public bool Exists(string sourceUrl)
        {
            using (var context = new Context())
            {
                var found = context.SourceDirectories.FirstOrDefault(x => x.SourceDirectoryUrl == sourceUrl);
                if (found == null)
                {
                    return false;
                }
                return true;
            }
        }

        public virtual async Task<IList<SourceDirectory>> GetAllAsync(
            params Expression<Func<SourceDirectory, object>>[] navigationProperties)
        {
            List<SourceDirectory> list;

            using (var context = new Context())
            {
                IQueryable<SourceDirectory> dbQuery = context.Set<SourceDirectory>();

                //Apply eager loading
                foreach (var navigationProperty in navigationProperties) dbQuery = dbQuery.Include(navigationProperty);

                list = await dbQuery.AsNoTracking().ToListAsync();
            }

            return list;
        }
    }
}