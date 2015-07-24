namespace Bookie.Common.Factories
{
    using System;
    using Model;

    public static class CoverImageFactory
    {
        public static CoverImage CreateNew()
        {
            var coverImage = new CoverImage
            {
                FileNameWithExtension = Guid.NewGuid() + ".jpg"
            };
            return coverImage;
        }

        public static CoverImage CreateEmpty()
        {
            return new CoverImage();
        }
    }
}