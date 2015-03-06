using System;
using System.Globalization;
using System.Windows.Data;

using DayInfo.Extensions;

namespace DayInfo.Converter
{
    public class Converter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value.ToString();

            switch ((String)parameter)
            {
                case "DateTime":
                    return ((DateTime) value).ToShortTimeString();

                case "EventType":
                    return Enum.Parse(typeof(SLServiceReference.WorkEventType), value.ToString(), true);

                case "TimeSpan":
                    return ((TimeSpan) value).TimeToString();
               
                default:
                    return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Converter.ConvertBack(..) need to be implemented");
        }

        #endregion
    }
}
