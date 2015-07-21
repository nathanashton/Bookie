namespace Bookie.Common.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Publisher : ITrackableEntity, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual int? BookId { get; set; }
        public virtual Book Book { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int? ModifiedUserId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}