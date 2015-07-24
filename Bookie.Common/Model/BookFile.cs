namespace Bookie.Common.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using static System.String;

    public class BookFile : IEntity
    {
        public int Id { get; set; }
        public string FileNameWithExtension { get; set; }

        [NotMapped]
        public string FullPathAndFileNameWithExtension
        {
            get
            {
                if (IsNullOrEmpty(FileNameWithExtension))
                {
                    return Empty;
                }
                return Book.SourceDirectory.SourceDirectoryUrl + "\\" + FileNameWithExtension;
            }
        }

        [NotMapped]
        public string FileExtension
        {
            get
            {
                if (IsNullOrEmpty(FileNameWithExtension))
                {
                    return Empty;
                }
                return Path.GetExtension(FileNameWithExtension);
            }
        }

        [NotMapped]
        public long FileSizeBytes
        {
            get
            {
                if (IsNullOrEmpty(FullPathAndFileNameWithExtension) ||
                    !File.Exists(FullPathAndFileNameWithExtension))
                {
                    return 0;
                }
                return new FileInfo(FullPathAndFileNameWithExtension).Length;
            }
        }

        public virtual Book Book { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}