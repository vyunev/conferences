using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using WAAD.POC.ProductCatalog.UWP.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WAAD.POC.ProductCatalog.UWP.View
{
    using Windows.UI.Xaml.Navigation;

    using Microsoft.ApplicationInsights;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewProductPage 
    {
        public ViewProductPage()
            :base(ViewModelType.ViewProduct)
        {
            InitializeComponent();
            btnAppBarPin.Click += Pin_Click;
            btnHeaderPin.Click += Pin_Click;
            btnHeaderUnPin.Click += UnPin_Click;
            btnAppBarUnPin.Click += UnPin_Click;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            var tc = new TelemetryClient();
            tc.TrackPageView(GetType().Name);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewProductViewModel;
            if (vm?.ProductDetails != null)
                await UnpinSecondaryTile(sender, vm);
        }

        private async void Pin_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewProductViewModel;
            if (vm?.ProductDetails != null)
                await PinSecondaryTile(sender, vm);
        }

        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private async Task PinSecondaryTile(object sender, ViewProductViewModel vm)
        {
            try
            {
                //Prepare the Tile metadata
                Uri smallLogo = new Uri("ms-appx://" + vm.ProductDetails.ImagePath);
                string tileActivationArguments = vm.ProductDetails.Id;
                string subTitle = vm.ProductDetails.Name;

                //Create the Secondary Tile and set the VisualElements options.
                SecondaryTile secondaryTile = new SecondaryTile(vm.ProductDetails.Id, subTitle, tileActivationArguments, smallLogo, TileSize.Square150x150);
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                secondaryTile.VisualElements.ForegroundText = ForegroundText.Dark;
                
                //Accent color for tile which will work if Foreground Text is either Black or White.
                secondaryTile.VisualElements.BackgroundColor = Color.FromArgb(255,27,161,226);
                
                //Attempt to Pin the Tile
                bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Above);

                //Set the Pinned State from the returned status (nb: don't wait until system can find the tile as it may not happen until a few MS later)
                vm.SetPinnedState(isPinned);

                //Notify the User of the Pinned Status.
                if (isPinned)
                {
                    MessageDialog dialog = new MessageDialog("Product " + vm.ProductDetails.Name + " successfully pinned.");
                    await dialog.ShowAsync();
                }
                else
                {
                    MessageDialog dialog = new MessageDialog("Product " + vm.ProductDetails.Name + " not pinned.");
                    await dialog.ShowAsync();
                }

            }
            catch (Exception e)
            {
                MessageDialog dialog = new MessageDialog("There was an error pinning this tile. \n\n Message : " + (e.Message??"(Error Details Unavailable)"), "Error Pinning Tile");
                await dialog.ShowAsync();
            }
        }

        /// <summary>
        /// This method unpins the existing secondary tile. 
        /// The user is shown a message informing whether the tile is unpinned successfully
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="vm">The Current Product View Model.</param>
        /// <returns></returns>
        private async Task UnpinSecondaryTile(object sender, ViewProductViewModel vm)
        {
            try
            {
                if (SecondaryTile.Exists(vm.ProductDetails.Id))
                {
                    SecondaryTile secondaryTile = new SecondaryTile(vm.ProductDetails.Id);

                    //nb: should always return true as we checked with Exists()
                    if (secondaryTile != null)
                    {
                        bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Above);

                        vm.SetPinnedState(!isUnpinned);

                        if (isUnpinned)
                        {
                            MessageDialog dialog = new MessageDialog("Product " + vm.ProductDetails.Name + " successfully unpinned.");
                            await dialog.ShowAsync();
                        }
                        else
                        {
                            MessageDialog dialog = new MessageDialog("Product " + vm.ProductDetails.Name + " not unpinned.");
                            await dialog.ShowAsync();
                        }

                    }
                }
                else
                {

                    MessageDialog dialog = new MessageDialog("The tile for Product " + vm.ProductDetails.Name + " was not located.");
                    await dialog.ShowAsync();

                }

            }
            catch (Exception e)
            {
                MessageDialog dialog = new MessageDialog("There was an error unpinning this tile. \n\n Message : " + (e.Message ?? "(Error Details Unavailable)"), "Error Unpinning Tile");
                await dialog.ShowAsync();
            }

        }

    }
}
