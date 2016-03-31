namespace WAAD.POC.ProductCatalog.UWP.Common
{
    /// <summary>
    /// Device Families
    /// </summary>
    public enum DeviceFamily
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Desktop
        /// </summary>
        Desktop = 1,
        /// <summary>
        /// Mobile
        /// </summary>
        Mobile = 2,
        /// <summary>
        /// Team
        /// </summary>
        Team = 3,
        /// <summary>
        /// Windows universal (for some reason this is returned by IoT
        /// </summary>
        Universal = 255
    }
}