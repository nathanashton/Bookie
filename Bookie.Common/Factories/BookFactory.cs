namespace Bookie.Common.Factories
{
    using System.Collections.Generic;
    using System.IO;
    using Model;
    using static System.String;

    public static class BookFactory
    {
        public static Book CreateNew(SourceDirectory sourceDirectory, string file)
        {
            var book = new Book
            {
                SourceDirectory = new SourceDirectory {Id = sourceDirectory.Id, EntityState = EntityState.Unchanged, SourceDirectoryUrl = sourceDirectory.SourceDirectoryUrl},
                CoverImage = CoverImageFactory.CreateEmpty(),
                BookFile = BookFileFactory.CreateNew(file),
                Title = Path.GetFileNameWithoutExtension(file),
                Abstract = Empty
            };
            book.SourceDirectory.EntityState = EntityState.Unchanged;
            book.BookFile.Book = book;
            book.BookHistory = new BookHistory {Book = book};
            book.Authors = new HashSet<Author>();
            book.Publishers = new HashSet<Publisher>();
            book.BookMarks = new HashSet<BookMark>();
            book.Notes = new HashSet<Note>();
            book.SourceDirectoryId = sourceDirectory.Id;
            return book;
        }
    }
}