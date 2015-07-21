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
    using System.Windows;

    internal class PageRowBound
    {
        public PageRowBound(Size size, double verticalOffset, double horizontalOffset)
        {
            Size = size;
            VerticalOffset = verticalOffset;
            HorizontalOffset = horizontalOffset;
        }

        public Size Size { get; }
        public double VerticalOffset { get; }
        public double HorizontalOffset { get; }

        public Size SizeIncludingOffset
        {
            get { return new Size(Size.Width + HorizontalOffset, Size.Height + VerticalOffset); }
        }
    }
}