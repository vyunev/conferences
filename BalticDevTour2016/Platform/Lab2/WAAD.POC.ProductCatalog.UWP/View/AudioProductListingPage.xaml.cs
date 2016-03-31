using WAAD.POC.ProductCatalog.UWP.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WAAD.POC.ProductCatalog.UWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AudioProductListingPage
    {
        public AudioProductListingPage()
            :base(ViewModelType.AudioProductListing)
        {
            InitializeComponent();
        }
    }
}
