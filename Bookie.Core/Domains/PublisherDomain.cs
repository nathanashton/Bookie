namespace Bookie.Core.Domains
{
    using System.Collections.Generic;
    using Common.Model;
    using Data.Interfaces;
    using Data.Repositories;
    using Interfaces;

    public class PublisherDomain : IPublisherDomain
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherDomain()
        {
            _publisherRepository = new PublisherRepository();
        }

        public IList<Publisher> GetAllPublishers()
        {
            return _publisherRepository.GetAll();
        }

        public Publisher GetPublisherByName(string publisherName)
        {
            return _publisherRepository.GetSingle(x => x.Name.Equals(publisherName));
        }

        public void AddPublisher(params Publisher[] publisher)
        {
            var tt = publisher;

            foreach (var b in publisher)
            {
                if (GetPublisherByName(b.Name) != null)
                {
                    // Exists
                }
            }
            _publisherRepository.Add(publisher);
        }

        public void UpdatePublisher(params Publisher[] publisher)
        {
            _publisherRepository.Update(publisher);
        }

        public void RemovePublisher(params Publisher[] publisher)
        {
            _publisherRepository.Remove(publisher);
        }
    }
}