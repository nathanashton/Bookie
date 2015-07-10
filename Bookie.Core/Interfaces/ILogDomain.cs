namespace Bookie.Core.Interfaces
{
    using Bookie.Common.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILogDomain
    {
        IList<LogEntity> GetAll();

        void AddLogEntry(params LogEntity[] logEntity);

        void RemoveLogEntry(params LogEntity[] logEntity);

        void RemoveAllEntrys();

        Task<IList<LogEntity>> GetAllAsync();
    }
}