namespace Bookie.Common.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SourceDirectory : ITrackableEntity, IEntity
    {
        public SourceDirectory()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string SourceDirectoryUrl { get; set; }
        public DateTime? DateLastImported { get; set; }
        public DateTime? DateLastScanned { get; set; }
        public virtual ICollection<Book> Books { get; set; }

        [NotMapped]
        public int BookCount
        {
            get
            {
                if (Books != null)
                {
                    return Books.Count;
                }
                return 0;
            }
        }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int? ModifiedUserId { get; set; }

        public override string ToString()
        {
            return SourceDirectoryUrl;
        }
    }
}