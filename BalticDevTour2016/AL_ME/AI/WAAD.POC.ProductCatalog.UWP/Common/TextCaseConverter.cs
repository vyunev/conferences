using System;
using Windows.UI.Xaml.Data;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public class TextCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return "";
            return value.ToString().ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
