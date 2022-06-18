using System;
using System.Globalization;
using System.Windows.Data;

namespace Tour_Planner.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        // https://stackoverflow.com/q/34337755
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string EnumString;
            try
            {
                EnumString = Enum.GetName(value.GetType(), value)!;
                return EnumString;
            }
            catch
            {
                return string.Empty;
            }
        }
        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
