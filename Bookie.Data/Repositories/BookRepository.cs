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

    public class BookRepository : GenericDataRepository<Book>, IBookRepository
    {
        public bool Exists(string filePath)
        {
            using (var context = new Context())
            {
                var found =
                    GetAll(x => x.BookFile, d => d.SourceDirectory)
                        .Where(p => p.BookFile.FullPathAndFileNameWithExtension == filePath);
                return found.Any();
            }
        }

        public virtual async Task<IList<Book>> GetAllAsync(params Expression<Func<Book, object>>[] navigationProperties)
        {
            List<Book> list;

            using (var context = new Context())
            {
                IQueryable<Book> dbQuery = context.Set<Book>();

                //Apply eager loading
                foreach (var navigationProperty in navigationProperties) dbQuery = dbQuery.Include(navigationProperty);

                list = await dbQuery.AsNoTracking().ToListAsync();
            }

            return list;
        }

        public List<Book> GetAllNested()
        {
            using (var ctx = new Context())
            {
                return
                    ctx.Books.Include(a => a.SourceDirectory)
                        .Include(b => b.BookFile)
                        .Include(c => c.CoverImage)
                        .Include(r => r.BookHistory)
                        .AsNoTracking()
                        .ToList();
            }
        }
    }
}