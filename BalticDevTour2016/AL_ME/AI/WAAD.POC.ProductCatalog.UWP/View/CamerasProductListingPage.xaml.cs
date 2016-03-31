using WAAD.POC.ProductCatalog.UWP.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WAAD.POC.ProductCatalog.UWP.View
{
    using Windows.UI.Xaml.Navigation;

    using Microsoft.ApplicationInsights;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CamerasProductListingPage
    {
        public CamerasProductListingPage()
            :base(ViewModelType.CamerasProductListing)
        {
            InitializeComponent();
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            var tc = new TelemetryClient();
            tc.TrackPageView(GetType().Name);
        }
    }
}
