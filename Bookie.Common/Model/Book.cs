namespace Bookie.Common.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Book : IEntity
    {
        public Book()
        {
            SourceDirectory = new SourceDirectory();
            BookFile = new BookFile();
            CoverImage = new CoverImage();
            BookHistory = new BookHistory();
            Authors = new HashSet<Author>();
            Publishers = new HashSet<Publisher>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public DateTime? DatePublished { get; set; }
        public int? Pages { get; set; }
        public string Isbn { get; set; }
        public bool Scraped { get; set; }
        public bool Favourite { get; set; }
        public int SourceDirectoryId { get; set; }
        public virtual SourceDirectory SourceDirectory { get; set; }
        public virtual BookFile BookFile { get; set; }
        public virtual CoverImage CoverImage { get; set; }
        public virtual BookHistory BookHistory { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Publisher> Publishers { get; set; }
        public virtual ICollection<BookMark> BookMarks { get; set; }
        public virtual ICollection<Note> Notes { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}