using System;
using System.Globalization;
using System.Windows.Data;

namespace ChromeBrowser.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool loading;
            if (null!=value&&bool.TryParse(value.ToString(), out loading)&&loading)
            {
                return "正在加载...";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
