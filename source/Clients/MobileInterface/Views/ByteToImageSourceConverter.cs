using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace MobileInterface.Views
{
    public class ByteToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is byte[] image)
            {
                return ImageSource.FromStream(() => new MemoryStream(image));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
