using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PostApp.Converters
{
    public class ImagePathToUrlConverter : IValueConverter
    {
        private static readonly string IMAGE_SERVER = ""; 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                if (!string.IsNullOrEmpty(parameter as string))
                    return $"{parameter.ToString()}_empty.jpg";
                else
                    return "";
            }
            return $"{IMAGE_SERVER}/{value.ToString()}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
