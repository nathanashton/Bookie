/*! MoonPdfLib - Provides a WPF user control to display PDF files
Copyright (C) 2013  (see AUTHORS file)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
!*/

namespace MoonPdfLib
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows;
    using Helper;
    using MuPdf;
    using Virtualizing;

    internal class PdfImageProvider : IItemsProvider<IEnumerable<PdfImage>>
    {
        private readonly string password;
        private readonly IPdfSource pdfSource;
        private readonly bool preFetch;
        private readonly int totalPages;
        private int count = -1;

        public PdfImageProvider(IPdfSource pdfSource, int totalPages, PageDisplaySettings settings, bool preFetch = true,
            string password = null)
        {
            this.pdfSource = pdfSource;
            this.totalPages = totalPages;
            Settings = settings;
            this.preFetch = preFetch; // preFetch is relevant for continuous page display
            this.password = password;
        }

        public PageDisplaySettings Settings { get; }

        public int FetchCount()
        {
            if (count == -1)
                count = MuPdfWrapper.CountPages(pdfSource, password);

            return count;
        }

        public IList<IEnumerable<PdfImage>> FetchRange(int startIndex, int count)
        {
            var imagesPerRow = Settings.ImagesPerRow;
            var viewType = Settings.ViewType;

            startIndex = (startIndex*imagesPerRow) + 1;

            if (preFetch)
                count = count*imagesPerRow;

            if (viewType == ViewType.BookView)
            {
                if (startIndex == 1)
                    count = Math.Min(totalPages, preFetch ? (1 /*first page*/+ imagesPerRow) : 0);
                else
                    startIndex--;
            }

            var end = Math.Min(FetchCount(), startIndex + count - 1);
            var list = new List<IEnumerable<PdfImage>>();
            var rowList = new List<PdfImage>(imagesPerRow);
            var offset = viewType == ViewType.BookView ? 1 : 0;

            for (var i = Math.Min(FetchCount(), startIndex);
                i <= Math.Min(FetchCount(), Math.Max(startIndex, end));
                i++)
            {
                var margin = new Thickness(0, 0, Settings.HorizontalOffsetBetweenPages, 0);

                using (var bmp = MuPdfWrapper.ExtractPage(pdfSource, i, Settings.ZoomFactor, password))
                {
                    if (Settings.Rotation != ImageRotation.None)
                    {
                        var flipType = RotateFlipType.Rotate90FlipNone;

                        if (Settings.Rotation != ImageRotation.Rotate90)
                            flipType = Settings.Rotation == ImageRotation.Rotate180
                                ? RotateFlipType.Rotate180FlipNone
                                : RotateFlipType.Rotate270FlipNone;

                        bmp.RotateFlip(flipType);
                    }

                    var bms = bmp.ToBitmapSource();
                    // Freeze bitmap to avoid threading problems when using AsyncVirtualizingCollection,
                    // because FetchRange is NOT called from the UI thread
                    bms.Freeze();

                    if ((i == 1 && viewType == ViewType.BookView) || (i + offset)%2 == 0)
                        margin.Right = 0;
                    // set right margin to zero for first page and for all pages that are on the right side

                    var img = new PdfImage {ImageSource = bms, Margin = margin};

                    // if first page and viewtype bookview, add the first page and continue with next
                    if (viewType == ViewType.BookView && i == 1)
                    {
                        list.Add(new[] {img});
                        continue;
                    }

                    rowList.Add(img);
                }

                // if all images per row were added or the end of the pdf is reached, add the remaining PdfImages from rowList to the final list
                if (rowList.Count%imagesPerRow == 0 || i == end)
                {
                    list.Add(rowList);

                    if (i == end && rowList.Count%imagesPerRow != 0)
                    {
                        var last = rowList.Last();
                        last.Margin = new Thickness(0);
                    }

                    if (i < end)
                        rowList = new List<PdfImage>(imagesPerRow);
                }
            }

            return list;
        }
    }

    public enum ImageRotation
    {
        None,
        Rotate90,
        Rotate180,
        Rotate270
    }
}