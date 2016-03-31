using WAAD.POC.ProductCatalog.UWP.ViewModel;

namespace WAAD.POC.ProductCatalog.UWP.View
{
    using Windows.UI.Xaml.Navigation;

    using Microsoft.ApplicationInsights;

    partial class HomePage 
    {
        public HomePage()
            :base(ViewModelType.Home)
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
