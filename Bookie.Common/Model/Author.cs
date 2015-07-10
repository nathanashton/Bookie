namespace Bookie.Common.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Author : ITrackableEntity, IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public virtual int? BookId { get; set; }

        public virtual Book Book { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public int? ModifiedUserId { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }
}