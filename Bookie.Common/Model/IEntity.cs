namespace Bookie.Common.Model
{
    using System;

    public interface IEntity
    {
        EntityState EntityState { get; set; }
        DateTime? CreatedDateTime { get; set; }
        DateTime? ModifiedDateTime { get; set; }
    }

    public enum EntityState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }
}