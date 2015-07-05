namespace Bookie.Common.Model
{
    using System;

    public interface ITrackableEntity
    {
        DateTime? CreatedDateTime { get; set; }

        int? CreatedUserId { get; set; }

        DateTime? ModifiedDateTime { get; set; }

        int? ModifiedUserId { get; set; }
    }
}