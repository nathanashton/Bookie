﻿namespace Bookie.Core.Domains
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Model;
    using Data.Interfaces;
    using Data.Repositories;
    using Interfaces;

    public class LogDomain : ILogDomain
    {
        private readonly ILogRepository _logRepository;

        public LogDomain()
        {
            _logRepository = new LogRepository();
        }

        public IList<LogEntity> GetAll()
        {
            return _logRepository.GetAll();
        }

        public void AddLogEntry(params LogEntity[] logEntity)
        {
            _logRepository.Add(logEntity);
        }

        public void RemoveLogEntry(params LogEntity[] logEntity)
        {
            _logRepository.Remove(logEntity);
        }

        public void RemoveAllEntrys()
        {
            _logRepository.RemoveAll();
        }

        public async Task<IList<LogEntity>> GetAllAsync()
        {
            return await _logRepository.GetAllAsync();
        }
    }
}