using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DirtyDucatsChecker
{
    public class RatioToBackgroundColorConverter : IValueConverter
    {
        private SolidColorBrush yellowColorBrush = new SolidColorBrush(Color.FromRgb(0xF9, 0xF9, 0x9F));
        private SolidColorBrush redColorBrush = new SolidColorBrush(Color.FromRgb(245, 137, 142));

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double input = System.Convert.ToDouble(value);
            if (input >= 15)
            {
                return Brushes.LightGreen;
            }
            else if (input >= 10)
            {
                return yellowColorBrush;
            }
            return redColorBrush;
            //return DependencyProperty.UnsetValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
