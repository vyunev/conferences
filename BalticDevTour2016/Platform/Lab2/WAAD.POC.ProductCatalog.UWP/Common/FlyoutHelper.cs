using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WAAD.POC.ProductCatalog.UWP.DisplayItems;
using WAAD.POC.ProductCatalog.UWP.Flyout;
using WAAD.POC.ProductCatalog.UWP.ViewModel;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public static class FlyoutHelper
    {
 
        public static async void ShowProductFilterFlyout(SubCategoryDisplayItem subcategoryDisplayItem, ProductListFilterViewModel fvm)
        {
            try
            {
                //Check if we are on Windows Mobile
                bool isWindowsPhone = DeviceFamilyHelper.CurrentDeviceFamily() == DeviceFamily.Mobile;

                //If Windows Phone then use the Dialog Control otherwise use the SettingsFlyout control.
                if (isWindowsPhone)
                {
                    ProductListFilterFlyout flyout = new ProductListFilterFlyout() {DataContext = fvm, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalContentAlignment = VerticalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch};
                    flyout.Margin = new Thickness(0, 20, 0, 0);
                    ContentDialog dialog = new ContentDialog()
                    {
                        Title = "Select Product Filters",
                        Content = flyout,
                        PrimaryButtonText = "Close",
                        FullSizeDesired = true,
                        Background = (SolidColorBrush)Application.Current.Resources["FlyoutBackgroundThemeBrush"],
                        Foreground = new SolidColorBrush(Colors.White),
                        RequestedTheme = ElementTheme.Dark,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Stretch
                    };

                    await dialog.ShowAsync();
                }
                else
                {
                    SettingsFlyout fly = new SettingsFlyout();
                    ProductListFilterFlyout flyout = new ProductListFilterFlyout() { DataContext = fvm }; //(subcategoryDisplayItem);
                    fly.Content = flyout;
                    fly.RequestedTheme = ElementTheme.Dark;
                    fly.Background = (SolidColorBrush)Application.Current.Resources["FlyoutBackgroundThemeBrush"];
                    fly.HeaderBackground = (SolidColorBrush)Application.Current.Resources["FlyoutBackgroundThemeBrush"];
                    fly.Title = "Filter Products";
                    fly.ShowIndependent();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
    }
}
