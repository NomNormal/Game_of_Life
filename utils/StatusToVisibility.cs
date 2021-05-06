using Game_of_Life.enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Game_of_Life
{

    class StatusToVisibility : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((EStatus)value == EStatus.ALIVE)
            {
                return Brushes.Black;
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fbe1d5"));
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
