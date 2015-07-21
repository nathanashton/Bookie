namespace Bookie.Core.Interfaces
{
    using System.Collections.Generic;
    using Common.Model;

    public interface ICoverImageDomain
    {
        IList<CoverImage> GetAllCoverImages();
        CoverImage GetCoverImageByUrl(string coverImageUrl);
        void AddCoverImage(params CoverImage[] coverimage);
        void UpdateCoverImage(params CoverImage[] coverimage);
        void RemoveCoverImage(params CoverImage[] coverimage);
        CoverImage GenerateCoverImageFromPdf(Book book);
        CoverImage EmptyCoverImage();
    }
}