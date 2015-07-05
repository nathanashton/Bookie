namespace Bookie.Common.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BookFile : ITrackableEntity, IEntity
    {
        public int Id { get; set; }

        public string FileNameWithExtension { get; set; }

        public string FullPathAndFileNameWithExtension { get; set; }

        public string FileExtension { get; set; }

        public long FileSizeBytes { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public int? ModifiedUserId { get; set; }

        public virtual Book Book { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}