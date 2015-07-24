namespace Bookie.Core
{
    using System.IO;
    using Common;
    using Domains;

    public class Library
    {
        private readonly CoverImageDomain _coverImageDomain = new CoverImageDomain();

        public void CleanImages()
        {
            Logger.Log.Debug("Cleaning Cover Images");
            var allCovers = Directory.GetFiles(Globals.CoverImageFolder, "*.jpg");
            foreach (var file in allCovers)
            {
                var existing = _coverImageDomain.GetCoverImageByUrl(Path.GetFileName(file));
                if (existing == null)
                {
                    File.Delete(file);
                    Logger.Log.Debug("Deleted " + file);
                }
            }
        }
    }
}