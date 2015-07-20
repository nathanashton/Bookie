using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Bookie.Converters
{
    public class CoverImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string coverPath;
            string v = "";
            try
            {
                v = value.ToString();
            }
            catch (NullReferenceException)
            {
                coverPath = "pack://application:,,,/Resources/Images/NoCoverAvailable.png";
            }
            if (File.Exists(v))
            {
                coverPath = v;
            }
            else
            {
                coverPath = "pack://application:,,,/Resources/Images/NoCoverAvailable.png";
            }

            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(coverPath);
            image.EndInit();

            return image;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}