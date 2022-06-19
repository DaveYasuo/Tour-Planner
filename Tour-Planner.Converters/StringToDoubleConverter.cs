using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                if (isDigit.Item1 && isDigit.Item2)
                {
                    if (ind == -1) return double.Parse(tmp);

                    if (ind != 0 && ind != length)
                    {
                        int dec = length - ind;
                        if (dec <= 3) return value;
                    }
                    if (ind == length)
                    {
                        return value;
                    }
                }
                return double.Parse(tmp.Remove(length));
            }
            catch
            {
                return 0;
            }
        }

        static (bool, bool) IsDigitsOnly(string str)
        {
            int dotCnt = 0;
            foreach (char c in str)
            {
                if ((c < '0' || c > '9') && c != '.')
                {
                    return (false, false);
                }
                if (c == '.')
                {
                    dotCnt++;
                }
            }
            return (true, dotCnt == 1 || dotCnt == 0);
        }

    }
}
