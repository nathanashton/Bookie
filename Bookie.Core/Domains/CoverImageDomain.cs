namespace Bookie.Core.Domains
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Common;
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





            if (!File.Exists(book.BookFile.FullPathAndFileNameWithExtension))
            {
                return EmptyCoverImage();
            }
            var outImageName = book.Title;
            // Strip invalid characters
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                outImageName = outImageName.Replace(c.ToString(), string.Empty);
            }
            outImageName += ".jpg";
            var savedImageUrl = Globals.CoverImageFolder + "\\" + outImageName;
            var file = new FileSource(book.BookFile.FullPathAndFileNameWithExtension);

            using (var img = MuPdfWrapper.ExtractPage(file, 1))
            {
                img.Save(savedImageUrl);

            }

            if (book.CoverImage == null)
            {
                book.CoverImage = new CoverImage();
            }

            if (book.Id == 0)
            {
                book.CoverImage.EntityState = EntityState.Added;
            }
            else
            {
                book.CoverImage.Id = book.CoverImage.Id;
                book.CoverImage.EntityState = EntityState.Modified;
            }

            book.CoverImage.FileNameWithExtension = Path.GetFileName(savedImageUrl);
            book.CoverImage.FullPathAndFileNameWithExtension = Globals.CoverImageFolder
                                                               + Path.GetFileNameWithoutExtension(
                                                                   savedImageUrl) + ".jpg";
            book.CoverImage.FileExtension = ".jpg";
            return book.CoverImage;
        }

        CoverImage ICoverImageDomain.EmptyCoverImage()
        {
            return EmptyCoverImage();
        }

        public static CoverImage EmptyCoverImage()
        {
            var cover = new CoverImage
            {
                FileNameWithExtension = Path.GetFileName(string.Empty),
                FullPathAndFileNameWithExtension = Globals.CoverImageFolder
                                                   + Path.GetFileNameWithoutExtension(
                                                       string.Empty) + ".jpg",
                FileExtension = ".jpg"
            };
            return cover;
        }
    }
}