namespace Bookie.Core.Domains
{
    using System.Collections.Generic;
    using Common.Model;
    using Data.Interfaces;
    using Data.Repositories;
    using Interfaces;

    public class ExcludedDomain : IExcludedDomain
    {
        private readonly IExcludedRepository _excludedRepository;

        public ExcludedDomain()
        {
            _excludedRepository = new ExcludedRepository();
        }

        public IList<Excluded> GetAllExcluded()
        {
            return _excludedRepository.GetAll();
        }

        public Excluded GetExcludedByUrl(string url)
        {
            return _excludedRepository.GetSingle(x => x.Url == url);
        }

        public void AddExcluded(params Excluded[] excluded)
        {
            _excludedRepository.Add(excluded);
        }

        public void UpdateExcluded(params Excluded[] excluded)
        {
            _excludedRepository.Update(excluded);
        }

        public void RemoveExcluded(params Excluded[] excluded)
        {
            _excludedRepository.Remove(excluded);
        }
    }
}