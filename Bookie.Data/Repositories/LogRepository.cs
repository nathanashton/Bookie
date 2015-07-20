using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Threading.Tasks;
using Bookie.Common;
using Bookie.Common.Model;
using Bookie.Data.Interfaces;

namespace Bookie.Data.Repositories
{
    public class LogRepository : GenericDataRepository<LogEntity>, ILogRepository
    {
        public void RemoveAll()
        {
            
          
        }

        public async Task<IList<LogEntity>> GetAllAsync()
        {
            List<LogEntity> Logs = new List<LogEntity>();
            SqlCeConnection _connection = new SqlCeConnection(Globals.DbConnectionString);
            _connection.Open();
            var sql = "SELECT * FROM LogEntities";

            SqlCeCommand selectCommand;
            using (selectCommand = new SqlCeCommand(sql, _connection))
            {

                using (var reader = await selectCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                                            LogEntity log = new LogEntity();

                   
                        DateTime? CreatedDate = null;
                        DateTime CreatedDate2;
                        DateTime? ModifiedDate = null;
                        DateTime ModifiedDate2;

                        bool s = DateTime.TryParse(reader["CreatedDateTime"].ToString(), out CreatedDate2);
                        if (s)
                        {
                            CreatedDate = CreatedDate2;
                            log.CreatedDateTime = CreatedDate;
                        }

                        bool s2 = DateTime.TryParse(reader["ModifiedDateTime"].ToString(), out ModifiedDate2);
                        if (s2)
                        {
                            ModifiedDate = ModifiedDate2;
                            log.ModifiedDateTime = ModifiedDate;
                        }



                    log.Id = Convert.ToInt64(reader["Id"].ToString());
                    log.Date = Convert.ToDateTime(reader["Date"].ToString());
                        log.Level = reader["Level"].ToString();
                    log.Thread = reader["Thread"].ToString();
                    log.Message = reader["Message"].ToString();
                    log.Exception = reader["Exception"].ToString();
                        var createdUserId = reader["CreatedUserId"].ToString();
                        var modifiedUserId = reader["ModifiedUserId"].ToString();

                        if (String.IsNullOrEmpty(createdUserId))
                        {
                            log.CreatedUserId = null;
                        }
                        else
                        {
                            log.CreatedUserId = Convert.ToInt32(reader["CreatedUserId"].ToString());
                        }

                        if (String.IsNullOrEmpty(modifiedUserId))
                        {
                            log.ModifiedUserId = null;
                        }
                        else
                        {
                            log.ModifiedUserId = Convert.ToInt32(reader["ModifiedUserId"].ToString());
                        }


                    Logs.Add(log);
                    }
                }
                return Logs;
            }
        }
    }
}