namespace Bookie.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Model;

    public interface ILogRepository : IGenericDataRepository<LogEntity>
    {
        void RemoveAll();
        Task<IList<LogEntity>> GetAllAsync();
    }
}