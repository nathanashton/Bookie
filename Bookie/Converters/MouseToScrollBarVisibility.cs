using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Bookie.Converters
{
    [ValueConversion(typeof(bool), typeof(ScrollBarVisibility))]
    internal sealed class MouseOverToScrollBarVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}