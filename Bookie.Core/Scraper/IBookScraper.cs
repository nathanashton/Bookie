namespace Bookie.Core.Scraper
{
    using Bookie.Common;
    using System.Collections.ObjectModel;

    public interface IBookScraper
    {
        SearchResult.Search SearchBy { get; set; }

        object SearchQuery { get; set; }

        ObservableCollection<SearchResult> SearchBooks(object searchQuery);
    }
}