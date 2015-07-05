namespace Bookie.Data.Repositories
{
    using Bookie.Common.Model;
    using Bookie.Data.Interfaces;
    using System.Data.SqlServerCe;

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
    }
}