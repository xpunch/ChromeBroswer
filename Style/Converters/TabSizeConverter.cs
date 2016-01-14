using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Style.Converters
{
    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            var tabControl = values[0] as TabControl;
            if (tabControl != null)
            {
                double width = (tabControl.ActualWidth - 150) / tabControl.Items.Count;
                return (width <= 1) ? 0 : (width - 1);
            }

            return 200;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
