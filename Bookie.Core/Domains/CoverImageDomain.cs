namespace Bookie.Core.Domains
{
    using Bookie.Common.Model;
    using Bookie.Core.Interfaces;
    using Bookie.Data.Interfaces;
    using Bookie.Data.Repositories;
    using System;
    using System.Collections.Generic;

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
            throw new NotImplementedException();
        }

        public void UpdateCoverImage(params CoverImage[] coverimage)
        {
            _coverImageRepository.Update(coverimage);
        }

        public void RemoveCoverImage(params CoverImage[] coverimage)
        {
            _coverImageRepository.Remove(coverimage);
        }
    }
}