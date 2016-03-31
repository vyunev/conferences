using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.ViewModel;

namespace WAAD.POC.ProductCatalog.UWP.View
{
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights;

    public class CommonPage : Page
    {
        public BaseViewModel CurrentViewModel { get; private set; }
        public ViewModelType CurrentViewModelType { get; }
        public string PageParam { get; set; }

        public CommonPage(ViewModelType vmType)
            :this()
        {
            //Cache our instance of our home page..
            if (vmType==ViewModelType.Home)
                NavigationCacheMode = NavigationCacheMode.Enabled;
            CurrentViewModel = null;
            CurrentViewModelType = vmType;
            PageParam = "";
        }

        public CommonPage()
        {
            //InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
        }

        // Data.CategoryDataSource src;

        private NavigationHelper _navigationHelper;
        private ObservableDictionary _defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel => _defaultViewModel;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper => _navigationHelper;




        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var param = "";// e.NavigationParameter.ToString();
            if (e.NavigationParameter != null)
                param = e.NavigationParameter.ToString();

            if (CurrentViewModel == null)
                CurrentViewModel = ViewModelFactory.CreateViewModel(CurrentViewModelType);

            if (CurrentViewModel != null)
            {                
                await CurrentViewModel.InitializeViewModel(param);
                CurrentViewModel.HasInitialized = true;
                DataContext = CurrentViewModel;
            }
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //var properties = new Dictionary<string, string>
            //                     {
            //                         { "pagename", e.SourcePageType.Name},
            //                         { "namespage", e.SourcePageType.Namespace},
            //                         { "direction", "to"},
            //                     };
            //var tc = new TelemetryClient();
            //tc.TrackEvent("Navigation", properties);
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var properties = new Dictionary<string, string>
                                 {
                                     { "pagename", e.SourcePageType.Name},
                                     { "namespage", e.SourcePageType.Namespace},
                                     { "direction", "from"},
                                 };
            var tc = new TelemetryClient();
            tc.TrackEvent("Navigation", properties);
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion



    }

}
