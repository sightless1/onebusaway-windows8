﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OneBusAway.Converters
{
    public class TimeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime)
            {
                DateTime time = (DateTime)value;
                if (time > DateTime.MinValue)
                {
                    bool includeAMPM = true;
                    if (parameter != null)
                    {
                        Boolean.TryParse(parameter as string, out includeAMPM);
                    }

                    return (includeAMPM)
                        ? time.ToString("h:mm tt")
                        : time.ToString("h:mm");
                }
            }

            return String.Empty;            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}