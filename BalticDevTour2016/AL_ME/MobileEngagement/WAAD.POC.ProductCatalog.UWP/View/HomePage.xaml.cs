using Microsoft.Azure.Engagement;
using WAAD.POC.ProductCatalog.UWP.ViewModel;

namespace WAAD.POC.ProductCatalog.UWP.View
{
    partial class HomePage 
    {
        public HomePage()
            :base(ViewModelType.Home)
        {
            InitializeComponent();
            var id = EngagementAgent.Instance.GetDeviceId();
        }
    }
}
