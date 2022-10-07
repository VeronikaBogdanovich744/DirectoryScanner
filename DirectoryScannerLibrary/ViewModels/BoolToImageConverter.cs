using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DirectoryScannerConverters
{ 
     public class BoolToImageConverter : IValueConverter
     {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                bool val = (bool)value;
                if (!val) //is file
                {
                    return new BitmapImage(new Uri("C://Users//Veronika//Documents//work//5 сем//СПП//LR3//file.png", UriKind.RelativeOrAbsolute));
                }
                else //is directory
                {
                    return new BitmapImage(new Uri("C://Users//Veronika//Documents//work//5 сем//СПП//LR3//folder.png", UriKind.RelativeOrAbsolute));
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
     }  
}
