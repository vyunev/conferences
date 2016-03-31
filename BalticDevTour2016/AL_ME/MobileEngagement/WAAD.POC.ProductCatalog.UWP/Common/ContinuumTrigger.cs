using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace WAAD.POC.ProductCatalog.UWP.Common
{

    public class ContinuumTrigger : StateTriggerBase
    {
        public string UIMode
        {
            get { return (string)GetValue(UIModeProperty); }
            set { SetValue(UIModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UIMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UIModeProperty =
            DependencyProperty.Register("UIMode", typeof(string), typeof(ContinuumTrigger), new PropertyMetadata(""));

        public ContinuumTrigger()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                WindowActivatedEventHandler windowactivated = null;
                windowactivated = (s, e) =>
                {
                    Window.Current.Activated -= windowactivated;
                    var currentUIMode = UIViewSettings.GetForCurrentView().UserInteractionMode.ToString();
                    SetActive(currentUIMode == UIMode);
                };
                Window.Current.Activated += windowactivated;
                Window.Current.SizeChanged += Current_SizeChanged;
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var currentUIMode = UIViewSettings.GetForCurrentView().UserInteractionMode.ToString();
            SetActive(currentUIMode == UIMode);
        }
    }
}
