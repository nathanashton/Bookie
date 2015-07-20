using System.Collections.Generic;
using System.Threading.Tasks;
using Bookie.Common.Model;

namespace Bookie.Data.Interfaces
{
    public interface ILogRepository : IGenericDataRepository<LogEntity>
    {
        void RemoveAll();

        Task<IList<LogEntity>> GetAllAsync();
    }
}