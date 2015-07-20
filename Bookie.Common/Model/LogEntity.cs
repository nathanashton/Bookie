using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookie.Common.Model
{
    public class LogEntity : ITrackableEntity, IEntity
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public string Thread { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDateTime { get; set; }

        public int? ModifiedUserId { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}