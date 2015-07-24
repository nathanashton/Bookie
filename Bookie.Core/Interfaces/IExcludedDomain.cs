namespace Bookie.Core.Interfaces
{
    using System.Collections.Generic;
    using Common.Model;

    public interface IExcludedDomain
    {
        IList<Excluded> GetAllExcluded();
        Excluded GetExcludedByUrl(string url);
        void AddExcluded(params Excluded[] excluded);
        void UpdateExcluded(params Excluded[] excluded);
        void RemoveExcluded(params Excluded[] excluded);
    }
}