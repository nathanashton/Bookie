namespace Bookie.Core
{
    using Bookie.Common.Model;

    using Ghostscript.NET;
    using global::Bookie.Common;

    public class GetPdfImage
    {
        public static string LoadImage(Book book, int pageNumber)
        {
            var inputPdfFile = book.BookFile.FullPathAndFileNameWithExtension;
            //  var outImageName = Path.GetFileNameWithoutExtension(inputPdfFile);
            var outImageName = book.Title + ".jpg";

            var dev = new GhostscriptJpegDevice(GhostscriptJpegDeviceType.Jpeg)
                                            {
                                                GraphicsAlphaBits
                                                    =
                                                    GhostscriptImageDeviceAlphaBits
                                                    .V_4,
                                                TextAlphaBits =
                                                    GhostscriptImageDeviceAlphaBits
                                                    .V_4,
                                                ResolutionXY =
                                                    new GhostscriptImageDeviceResolution
                                                    (60, 60),
                                                JpegQuality = 80
                                            };
            dev.InputFiles.Add(inputPdfFile);
            dev.Pdf.FirstPage = pageNumber;
            dev.Pdf.LastPage = pageNumber;
            dev.CustomSwitches.Add("-dDOINTERPOLATE");
            dev.OutputPath = Globals.CoverImageFolder + "\\" + outImageName;
            dev.Process();

            return dev.OutputPath;
        }
    }
}