using Windows.System.Profile;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public static class DeviceFamilyHelper
    {
        public static DeviceFamily CurrentDeviceFamily()
        {
            try
            {
                return ResolveDeviceFamily(AnalyticsInfo.VersionInfo.DeviceFamily);
            }
            catch
            { }

            return DeviceFamily.Unknown;
        }

        private static DeviceFamily ResolveDeviceFamily(string deviceFamily)
        {
            string safeDeviceFamily = deviceFamily ?? "";
            if (safeDeviceFamily == "Windows.Mobile")
                return DeviceFamily.Mobile;
            else if (safeDeviceFamily == "Windows.Desktop")
                return DeviceFamily.Desktop;
            else if (safeDeviceFamily == "Windows.Team")
                return DeviceFamily.Team;
            else if (safeDeviceFamily == "Windows.Universal")
                return DeviceFamily.Universal;
            else
                return DeviceFamily.Unknown;
        }
    }
}
