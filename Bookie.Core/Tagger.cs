using System.Collections.Generic;
using System.Linq;
using Bookie.Core.Domains;

namespace Bookie.Core
{
    public class Tagger
    {
        private readonly BookDomain _bookDomain = new BookDomain();

        private readonly List<string> words = new List<string>();

        private readonly string[] _removedwords = {
                                                "edition", "for", "and", "with", "in", "the", "of", "to","1","2","3","4","5","6","7","8","9","0","by","1st","2nd","3rd","4th","5th","6th", " "
                                            };


        public List<string> Go()
        {
            var allBooks = _bookDomain.GetAllBooks();
            foreach (var book in allBooks)
            {
                var exploded = book.Title.Split(' ');
                foreach (var s in exploded)
                {
                    if (!_removedwords.Contains(s.ToLower()))
                    {
                        words.Add(s);
                    }
                }
            }
            var grouped = words.GroupBy(s => s).Select(group => new { Word = group.Key, Count = group.Count() }).ToList();
            grouped.Sort((a,b) => b.Count - a.Count);
            return words;
        }
    }
}
