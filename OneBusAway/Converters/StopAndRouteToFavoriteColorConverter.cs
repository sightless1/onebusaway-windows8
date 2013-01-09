﻿using OneBusAway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace OneBusAway.Converters
{
    public class StopAndRouteToFavoriteColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            StopAndRoutePair pair = (StopAndRoutePair)value;

            bool isFavorite = Favorites.IsFavorite(pair);

            if (isFavorite)
            {
                return new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0xAA, 0x36));
            }

            return new SolidColorBrush(Color.FromArgb(0xFF, 0xCF, 0xCF, 0xCF));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}