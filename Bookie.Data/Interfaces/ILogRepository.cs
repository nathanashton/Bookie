namespace Bookie.Data.Interfaces
{
    using Bookie.Common.Model;

    public interface ILogRepository : IGenericDataRepository<LogEntity>
    {
        void RemoveAll();
    }
}