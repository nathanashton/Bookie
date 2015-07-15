namespace Bookie.Data.Interfaces
{
    using Bookie.Common.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface ILogRepository : IGenericDataRepository<LogEntity>
    {
        void RemoveAll();

        Task<IList<LogEntity>> GetAllAsync();
    }
}