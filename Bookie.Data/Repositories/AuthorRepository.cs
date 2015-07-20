using Bookie.Common.Model;
using Bookie.Data.Interfaces;

namespace Bookie.Data.Repositories
{
    public class AuthorRepository : GenericDataRepository<Author>, IAuthorRepository
    {
    }
}