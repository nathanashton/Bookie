namespace Bookie.Core.Domains
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Common.Factories;
    using Common.Model;
    using Data.Interfaces;
    using Data.Repositories;
    using Interfaces;
    using MoonPdfLib.MuPdf;

    public class CoverImageDomain : ICoverImageDomain
    {
        private readonly ICoverImageRepository _coverImageRepository;

        public CoverImageDomain()
        {
            _coverImageRepository = new CoverImageRepository();
        }

        public IList<CoverImage> GetAllCoverImages()
        {
            return _coverImageRepository.GetAll();
        }

        public CoverImage GetCoverImageByUrl(string coverImageUrl)
        {
            return _coverImageRepository.GetSingle(x => x.FullPathAndFileNameWithExtension.Equals(coverImageUrl));
        }

        public void AddCoverImage(params CoverImage[] coverimage)
        {
            foreach (var b in coverimage)
            {
                b.CreatedDateTime = DateTime.Now;
                b.ModifiedDateTime = DateTime.Now;
            }
            _coverImageRepository.Add(coverimage);
        }

        public void UpdateCoverImage(params CoverImage[] coverimage)
        {
            _coverImageRepository.Update(coverimage);
        }

        public void RemoveCoverImage(params CoverImage[] coverimage)
        {
            _coverImageRepository.Remove(coverimage);
        }

        public CoverImage GenerateCoverImageFromPdf(Book book)
        {
            // If Book not found then return null;
            if (!File.Exists(book.BookFile.FullPathAndFileNameWithExtension))
            {
                return CoverImageFactory.CreateEmpty();
            }

            var coverImage = CoverImageFactory.CreateNew();
            using (var img = MuPdfWrapper.ExtractPage(new FileSource(book.BookFile.FullPathAndFileNameWithExtension), 1)
                )
            {
                img.Save(coverImage.FullPathAndFileNameWithExtension);
            }

            if (book.Id == 0)
            {
                coverImage.EntityState = EntityState.Added;
            }
            else
            {
                coverImage.Id = book.CoverImage.Id;
                coverImage.EntityState = EntityState.Modified;
            }
            return coverImage;
        }

        CoverImage ICoverImageDomain.EmptyCoverImage()
        {
            return EmptyCoverImage();
        }

        public static CoverImage EmptyCoverImage()
        {
            var cover = new CoverImage
            {
                FileNameWithExtension = Path.GetFileName(string.Empty)
            };
            return cover;
        }
    }
}