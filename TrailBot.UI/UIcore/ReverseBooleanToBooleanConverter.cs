using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CascadePass.TrailBot.UI
{
    public class ReverseBooleanToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is not bool)
            {
                return DependencyProperty.UnsetValue;
            }

            bool effectiveValue = !(bool)value;

            return effectiveValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value is not bool)
            {
                return DependencyProperty.UnsetValue;
            }

            bool effectiveValue = !(bool)value;

            return effectiveValue;
        }
    }
}
