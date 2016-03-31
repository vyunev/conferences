using Windows.UI.Xaml.Controls;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.UWP.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WAAD.POC.ProductCatalog.UWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CompareProductsPage 
    {
        public CompareProductsPage()
            :base(ViewModelType.CompareProducts)
        {
            InitializeComponent();
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(ViewProductPage), Parameter = (e.ClickedItem as Product).Id });
        }
    }
}
