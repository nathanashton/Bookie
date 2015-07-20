using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookie.Common.Model
{
    public class BookMark : IEntity, ITrackableEntity
    {
        public int Id { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public int? ModifiedUserId { get; set; }

        public int BookMarkedPage { get; set; }

        public virtual int? BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}