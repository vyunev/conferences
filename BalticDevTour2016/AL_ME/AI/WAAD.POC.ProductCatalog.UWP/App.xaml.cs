﻿using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.DataSources;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.View;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace WAAD.POC.ProductCatalog.UWP
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        //public static TelemetryClient Telemetry;

        TelemetryClient tc = new TelemetryClient();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {

            // App Insights Initialisation
            WindowsAppInitializer.InitializeAsync("0d096d33-a1d7-4b2d-82d9-15c176406fd9", 
                WindowsCollectors.Metadata | 
                WindowsCollectors.PageView | 
                WindowsCollectors.Session |
                WindowsCollectors.UnhandledException);

            //Telemetry.InstrumentationKey = ;
            tc.TrackEvent("Browsing Start");

            InitializeComponent();
            Suspending += OnSuspending;

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            tc.TrackTrace("App is launched");

#if DEBUG
            if (Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = false;
            }
#endif

            //Initialize the Data Manager + Default Favorites collection
            DataManager.Initialize(new LocalStoreDataService<ProductCategory>(),
                                new LocalStoreDataService<Product>(), new LocalStoreDataService<string>());
            FavoritesHelper.DefaultFavorites = (await DataManager.DefaultFavoritesDataSource.GetAllAsync()).ToList();


            //attempt to get current Toot Frame
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                // Set the default language
                rootFrame.Language = ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
            }

            // Place the frame in the current Window
            Window.Current.Content = new AppShell(rootFrame);
            string param = e.Arguments ?? "";

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter

                // If a launch parameter is provided (ie. from Secondary Tile), navigate to the product Page 
                // - otherwise go to the Home page.
                rootFrame.Navigate(param != "" ? typeof(ViewProductPage) : typeof(HomePage), e.Arguments);
            }
            else
            {
                if (param != "")
                    AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(ViewProductPage), Parameter = param });
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            tc.TrackTrace("App is suspended", SeverityLevel.Warning);
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
