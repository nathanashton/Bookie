namespace Bookie.Core.Scraper
{
    using System.Collections.ObjectModel;
    using Common;

    public interface IBookScraper
    {
        SearchResult.Search SearchBy { get; set; }
        object SearchQuery { get; set; }
        ObservableCollection<SearchResult> SearchBooks(object searchQuery);
    }
}