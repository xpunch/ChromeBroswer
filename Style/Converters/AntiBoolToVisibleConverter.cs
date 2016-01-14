using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Style.Converters
{
    class AntiBoolToVisibleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }

            return (object)(Visibility)(flag ? 2 : 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is Visibility)
            {
                return (Visibility)value != Visibility.Visible;
            }

            return false;
        }
    }
}
