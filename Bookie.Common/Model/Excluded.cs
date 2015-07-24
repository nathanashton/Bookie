namespace Bookie.Common.Model
{
    using System;

    public class Excluded : IEntity
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public EntityState EntityState { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}