using System.Collections.ObjectModel;
using Bookie.Common;

namespace Bookie.Core.Scraper
{
    public interface IBookScraper
    {
        SearchResult.Search SearchBy { get; set; }

        object SearchQuery { get; set; }

        ObservableCollection<SearchResult> SearchBooks(object searchQuery);
    }
}