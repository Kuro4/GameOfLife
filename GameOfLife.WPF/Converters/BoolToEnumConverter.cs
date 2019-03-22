using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GameOfLife.WPF.Converters
{
    public class BoolToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Enum.IsDefined(value.GetType(), value) || !(parameter is string parameterString)) return DependencyProperty.UnsetValue;
            object paramvalue = Enum.Parse(value.GetType(), parameterString);
            return (int)paramvalue == (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string paramStr) return Enum.Parse(targetType, paramStr);
            return DependencyProperty.UnsetValue;
        }
    }
}
