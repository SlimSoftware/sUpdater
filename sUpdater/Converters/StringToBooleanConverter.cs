using System.Globalization;
using System.Windows.Data;
using System;

namespace sUpdater.Converters
{
    /// <summary>
    /// Returns true if string has a value, false if null or empty
    /// </summary>
    public class StringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            return !string.IsNullOrEmpty(input);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}