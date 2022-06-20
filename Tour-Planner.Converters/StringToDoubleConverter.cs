using System;
using System.Globalization;
using System.Windows.Data;

namespace Tour_Planner.Converters
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string tmp = (string)value;
                int length = tmp.Length - 1;
                int ind = tmp.IndexOf('.');
                var isDigit = IsDigitsOnly(tmp);
                if (!isDigit.Item1 || !isDigit.Item2) return double.Parse(tmp.Remove(length));
                if (ind == -1) return double.Parse(tmp);

                if (ind == 0 || ind == length) return ind == length ? value : double.Parse(tmp.Remove(length));
                int dec = length - ind;
                if (dec <= 3) return double.Parse(tmp);
                return ind == length ? value : double.Parse(tmp.Remove(length));
            }
            catch
            {
                return 0;
            }
        }

        private static (bool, bool) IsDigitsOnly(string str)
        {
            int dotCnt = 0;
            foreach (var c in str)
            {
                switch (c)
                {
                    case < '0' or > '9' when c != '.':
                        return (false, false);
                    case '.':
                        dotCnt++;
                        break;
                }
            }
            return (true, dotCnt is 1 or 0);
        }

    }
}
