namespace Bookie.Core.Interfaces
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public interface IPublisherDomain
    {
        IList<Publisher> GetAllPublishers();

        Publisher GetPublisherByName(string publisherName);

        void AddPublisher(params Publisher[] publisher);

        void UpdatePublisher(params Publisher[] publisher);

        void RemovePublisher(params Publisher[] publisher);
    }
}