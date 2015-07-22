namespace Bookie.Common.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Note : IEntity
    {
        public int Id { get; set; }
        public string NoteText { get; set; }
        public int? PageNumber { get; set; }
        public virtual int? BookId { get; set; }
        public virtual Book Book { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}