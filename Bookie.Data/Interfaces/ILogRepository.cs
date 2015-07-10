namespace Bookie.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Bookie.Common.Model;

    public interface ILogRepository : IGenericDataRepository<LogEntity>
    {
        void RemoveAll();

        Task<IList<LogEntity>> GetAllAsync(params Expression<Func<LogEntity, object>>[] navigationProperties);
    }
}