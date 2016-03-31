using System;
using Windows.System.Profile;
using Windows.UI.Xaml;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public class DeviceFamilyAdaptiveTrigger : StateTriggerBase, ITriggerValue
    {
        private static string deviceFamily;

        static DeviceFamilyAdaptiveTrigger()
        {
            deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
        }

        /// <summary>
        /// Gets or sets the device family to trigger on.
        /// </summary>
        /// <value>The device family.</value>
        public DeviceFamily DeviceFamily
        {
            get { return (DeviceFamily)GetValue(DeviceFamilyProperty); }
            set { SetValue(DeviceFamilyProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DeviceFamily"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty DeviceFamilyProperty =
            DependencyProperty.Register("DeviceFamily", typeof(DeviceFamily), typeof(DeviceFamilyAdaptiveTrigger),
                new PropertyMetadata(DeviceFamily.Unknown, OnDeviceTypePropertyChanged));

        private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (DeviceFamilyAdaptiveTrigger)d;
            var val = (DeviceFamily)e.NewValue;
            if (deviceFamily == "Windows.Mobile")
                obj.IsActive = (val == DeviceFamily.Mobile);
            else if (deviceFamily == "Windows.Desktop")
                obj.IsActive = (val == DeviceFamily.Desktop);
            else if (deviceFamily == "Windows.Team")
                obj.IsActive = (val == DeviceFamily.Team);
            else if (deviceFamily == "Windows.Universal")
                obj.IsActive = (val == DeviceFamily.Universal);
            else
                obj.IsActive = (val == DeviceFamily.Unknown);
        }

        #region ITriggerValue

        private bool _mIsActive;

        /// <summary>
        /// Gets a value indicating whether this trigger is active.
        /// </summary>
        /// <value><c>true</c> if this trigger is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get { return _mIsActive; }
            private set
            {
                if (_mIsActive != value)
                {
                    _mIsActive = value;
                    SetActive(value);
                    IsActiveChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Occurs when the <see cref="IsActive" /> property has changed.
        /// </summary>
        public event EventHandler IsActiveChanged;

        #endregion ITriggerValue
    }
}