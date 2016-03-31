using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WAAD.POC.ProductCatalog.UWP.CustomControls
{
    public class ExtendedGridView : GridView
    {
        public ObservableCollection<object> BindableSelectedItems
        {
            get { return GetValue(BindableSelectedItemsProperty) as ObservableCollection<object>; }
            set { SetValue(BindableSelectedItemsProperty, value as ObservableCollection<object>); }
        }
        public static readonly DependencyProperty BindableSelectedItemsProperty =
            DependencyProperty.Register("BindableSelectedItems",
            typeof(ObservableCollection<object>), typeof(ExtendedGridView),
            new PropertyMetadata(null, (s, e) =>
            {
                (s as ExtendedGridView).SelectionChanged -= (s as ExtendedGridView).MyGridView_SelectionChanged;
                (s as ExtendedGridView).SelectionChanged += (s as ExtendedGridView).MyGridView_SelectionChanged;
            }));
        void MyGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BindableSelectedItems == null)
                return;
            foreach (var item in BindableSelectedItems.Where(x => !SelectedItems.Contains(x)).ToArray())
                BindableSelectedItems.Remove(item);
            foreach (var item in SelectedItems.Where(x => !BindableSelectedItems.Contains(x)))
                BindableSelectedItems.Add(item);
        }

    }
}
