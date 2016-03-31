using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using WAAD.POC.ProductCatalog.UWP.Common;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WAAD.POC.ProductCatalog.UWP.CustomControls
{
    public sealed partial class HeaderControl 
    {
        bool _isWindowsPhone = DeviceFamilyHelper.CurrentDeviceFamily() == DeviceFamily.Mobile;

        //Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");

        bool _isForcedCompactSearch = false;

        public HeaderControl()
        {
            InitializeComponent();

            titleBar.Margin = _isWindowsPhone ? new Thickness(48, 0, 0, 0) : new Thickness(20, 0, 0, 0);
            btnSearch.Click += (sender, e) =>
            {
                SearchUserControl srchPopup = new SearchUserControl();
                Popup flyOut = new Popup();
                flyOut.Child = srchPopup;
                flyOut.IsLightDismissEnabled = true;
                flyOut.HorizontalOffset = 48;
                flyOut.VerticalOffset = _isWindowsPhone ? 24 : 0;
                flyOut.Height = 48;
                flyOut.Width = Window.Current.Bounds.Width - 48;
                srchPopup.Width = flyOut.Width;
                srchPopup.Height = flyOut.Height;
                srchPopup.ForceClose += delegate {
                    flyOut.IsOpen = false;
                };
                flyOut.IsOpen = true;
                
            };

            SizeChanged += (s, sz) => {
                _isForcedCompactSearch = (sz.NewSize.Width < 500);
                FixSearchControl();
            };

            Loaded += (s, a) =>
            {
                FixSearchControl();
            };
        }

        public static readonly DependencyProperty IsCompactSearchModeProperty =
                    DependencyProperty.Register("IsCompactSearchMode", typeof(bool), typeof(HeaderControl), new PropertyMetadata(false));

        public bool IsCompactSearchMode
        {
            get { return (bool)GetValue(IsCompactSearchModeProperty); }
            set {
                SetValue(IsCompactSearchModeProperty, value);
                FixSearchControl();
            }
        }

        private void SetSearchMode(bool isCompact)
        {           
            srchControl.Visibility = isCompact ? Visibility.Collapsed : Visibility.Visible;
            btnSearch.Visibility = (!isCompact) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void FixSearchControl()
        {
            SetSearchMode(_isWindowsPhone || (IsCompactSearchMode)||(_isForcedCompactSearch));
        }


        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(UIElement), typeof(HeaderControl), new PropertyMetadata(DependencyProperty.UnsetValue));

        public UIElement ButtonsContent
        {
            get { return (UIElement)GetValue(ButtonsContentProperty); }
            set { SetValue(ButtonsContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonsContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonsContentProperty =
            DependencyProperty.Register("ButtonsContent", typeof(UIElement), typeof(HeaderControl), new PropertyMetadata(DependencyProperty.UnsetValue));
    }
}

