using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WAAD.POC.ProductCatalog.UWP.View
{
    using Microsoft.ApplicationInsights;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            Version v = GetAppAssemblyVersion();
            tbVersion.Text = 
                string.Format(
                    "Version {0}.{1}.{2}.{3}",
                    v.Major, v.Minor, v.Build, v.Revision
                );
        }

        private Version GetAppAssemblyVersion()
        {
            Type t = typeof(App);
            Assembly assembly = t.GetTypeInfo().Assembly;
            Version version = assembly.GetName().Version;
            return version;
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            var tc = new TelemetryClient();
            tc.TrackPageView(GetType().Name);
        }
    }
}
