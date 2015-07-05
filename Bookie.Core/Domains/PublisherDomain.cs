namespace Bookie.Core.Domains
{
    using Bookie.Common.Model;
    using Bookie.Core.Interfaces;
    using Bookie.Data.Interfaces;
    using Bookie.Data.Repositories;
    using iTextSharp.text;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public List<PublisherTreeView> GetPublisherTree()
        {
            var allBooks = new BookDomain().GetAllBooks().ToList();

            List<PublisherTreeView> Publishers = new List<PublisherTreeView>();

            var allPublishers = _publisherRepository.GetAll(x => x.Book).ToList();

            HashSet<string> elements = new HashSet<string>(); // Type of property
            allPublishers.RemoveAll(i => !elements.Add(i.Name));

            foreach (var publisher in allPublishers)
            {
                PublisherTreeView tree = new PublisherTreeView();
                tree.Publisher = publisher;
                foreach (var book in allBooks)
                {
                    foreach (var pub in book.Publishers)
                    {
                        if (pub.Name == publisher.Name)
                        {
                            tree.Books.Add(book);
                        }
                    }
                }
                Publishers.Add(tree);
            }
            return Publishers;
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
                b.CreatedDateTime = DateTime.Now;
                b.ModifiedDateTime = DateTime.Now;
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