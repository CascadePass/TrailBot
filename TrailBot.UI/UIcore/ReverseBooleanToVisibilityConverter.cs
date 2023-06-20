using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CascadePass.TrailBot.UI
{
    public class ReverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is not bool)
            {
                return Visibility.Collapsed;
            }

            bool effectiveValue = !(bool)value;

            return effectiveValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
