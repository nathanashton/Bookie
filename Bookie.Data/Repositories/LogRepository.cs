namespace Bookie.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using Bookie.Common.Model;
    using Bookie.Data.Interfaces;
    using System.Data.SqlServerCe;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class LogRepository : GenericDataRepository<LogEntity>, ILogRepository
    {
        public void RemoveAll()
        {
            try
            {
                using (var context = new Context())
                {
                    context.Database.ExecuteSqlCommand("DELETE FROM Logs");
                    context.SaveChanges();
                }
            }
            catch (SqlCeException)
            {
            }
        }

        public async Task<IList<LogEntity>> GetAllAsync(params Expression<Func<LogEntity, object>>[] navigationProperties)
        {
            List<LogEntity> list;

            using (var context = new Context())
            {
                IQueryable<LogEntity> dbQuery = context.Set<LogEntity>();

                //Apply eager loading
                foreach (var navigationProperty in navigationProperties) dbQuery = dbQuery.Include(navigationProperty);

                list = await dbQuery.AsNoTracking().ToListAsync();
            }

            return list;
        }
    }
}