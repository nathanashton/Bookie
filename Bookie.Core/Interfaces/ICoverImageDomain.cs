namespace Bookie.Core.Interfaces
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public interface ICoverImageDomain
    {
        IList<CoverImage> GetAllCoverImages();

        CoverImage GetCoverImageByUrl(string coverImageUrl);

        void AddCoverImage(params CoverImage[] coverimage);

        void UpdateCoverImage(params CoverImage[] coverimage);

        void RemoveCoverImage(params CoverImage[] coverimage);
    }
}