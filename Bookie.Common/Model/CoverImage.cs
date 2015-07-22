namespace Bookie.Common.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using static System.String;

    public class CoverImage : IEntity
    {
        public int Id { get; set; }

        [NotMapped]
        private string _fileNameWithExtension ;
        public string FileNameWithExtension
        {
            get
            {
                return IsNullOrEmpty(_fileNameWithExtension) ? Empty : _fileNameWithExtension;
            }
            set { _fileNameWithExtension = value; }
        }


        [NotMapped]
        public string FullPathAndFileNameWithExtension
        {
            get
            {
                if (IsNullOrEmpty(FileNameWithExtension))
                {
                    return Empty;
                }
                return Globals.CoverImageFolder + "\\" + FileNameWithExtension;
            }
        }

        [NotMapped]
        public string FileExtension => IsNullOrEmpty(FileNameWithExtension) ? Empty : Path.GetExtension(FileNameWithExtension);

        public virtual Book Book { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}