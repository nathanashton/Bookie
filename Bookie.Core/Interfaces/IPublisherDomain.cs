using System.Collections.Generic;
using Bookie.Common.Model;

namespace Bookie.Core.Interfaces
{
    public interface IPublisherDomain
    {
        IList<Publisher> GetAllPublishers();

        Publisher GetPublisherByName(string publisherName);

        void AddPublisher(params Publisher[] publisher);

        void UpdatePublisher(params Publisher[] publisher);

        void RemovePublisher(params Publisher[] publisher);
    }
}