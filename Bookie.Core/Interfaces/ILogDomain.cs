namespace Bookie.Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Model;

    public interface ILogDomain
    {
        IList<LogEntity> GetAll();
        void AddLogEntry(params LogEntity[] logEntity);
        void RemoveLogEntry(params LogEntity[] logEntity);
        void RemoveAllEntrys();
        Task<IList<LogEntity>> GetAllAsync();
    }
}