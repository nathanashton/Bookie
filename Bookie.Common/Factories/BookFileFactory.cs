namespace Bookie.Common.Factories
{
    using System.IO;
    using Model;

    public static class BookFileFactory
    {
        public static BookFile CreateNew(string file)
        {
            var bookFile = new BookFile();
            bookFile.FileNameWithExtension = Path.GetFileName(file);
            return bookFile;
        }
    }
}