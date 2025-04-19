using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SpoClient.Views.Extensions
{
    public class CurrentCultureToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var cuurrentCulture = new CultureInfo(parameter?.ToString() ?? value?.ToString() ?? string.Empty);
            if (cuurrentCulture is CultureInfo cultureInfo)
            {
                return cultureInfo.Equals(culture);
            }
            return false;
        }


        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isCurrentCulture && isCurrentCulture)
            {
                return parameter;
            }
            return null;
        }
    }
}
