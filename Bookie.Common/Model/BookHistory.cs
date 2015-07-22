namespace Bookie.Common.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BookHistory : IEntity
    {
        public int Id { get; set; }
        public DateTime? DateImported { get; set; }
        public DateTime? DateLastOpened { get; set; }
        public int? CurrentPage { get; set; }
        public virtual Book Book { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}