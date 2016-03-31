using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public sealed class BooleanToMultiSelectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? ListViewSelectionMode.Multiple : ListViewSelectionMode.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is ListViewSelectionMode && (ListViewSelectionMode)value != ListViewSelectionMode.None;
        }
    }
}