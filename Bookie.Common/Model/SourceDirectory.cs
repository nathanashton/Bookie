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

        public DateTime? CreatedDateTime { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public int? ModifiedUserId { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        [NotMapped]
        public int BookCount
        {
            get
            {
                if (Books != null)
                {
                    return Books.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        public override string ToString()
        {
            return SourceDirectoryUrl;
        }
    }
}