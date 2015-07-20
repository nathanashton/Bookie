using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookie.Common.Model
{
    public class BookHistory : ITrackableEntity, IEntity
    {
        public int Id { get; set; }

        public DateTime? DateImported { get; set; }

        public DateTime? DateLastOpened { get; set; }

        public int? CurrentPage { get; set; }

        public virtual Book Book { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public int? ModifiedUserId { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}