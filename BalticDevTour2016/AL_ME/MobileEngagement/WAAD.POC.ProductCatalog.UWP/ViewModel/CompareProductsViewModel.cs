using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.DataSources;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.Data;

namespace WAAD.POC.ProductCatalog.UWP.ViewModel
{
    //View Model for comparing two or more Products side by side.
    public class CompareProductsViewModel : BaseViewModel
    {
        public string PageName => "Compare Products";
        public ObservableCollection<Product> Products { get; set; }

        //Initialize the ViewModel.
        // - incoming page parameter will be a serialized list of Product Id's.
        public override async Task InitializeViewModel(string param)
        {           
            if (HasInitialized) return;
            Products.Clear();
            if (param != "")
            {               
                //Get the list of Product Ids
                List<string> productIds = param.DeserializeStringList();
                if (productIds.Count > 0)
                {
                    //Get the Required Products
                    var productsToCompare = await DataManager.ProductDataSource.GetProductsByProductIdsAsync(productIds);
                    if (productsToCompare.Count>0)
                        foreach (var product in productsToCompare)
                        {
                            //Add items one at a time for the Observable Products Collection.
                            Products.Add(product);
                        }
                }
            }            
        }

        public CompareProductsViewModel()
        {
            //Initialize our Products Collection
            Products = new ObservableCollection<Product>();

            //Generate some Design Time Data so we can use page in Blend
            if (DesignMode.DesignModeEnabled)
            {
                for (int idx=0;idx<5;idx++)
                {
                    var product = DesignTimeDataService.GenerateProduct();
                    product.Id = idx.ToString();
                    product.Name = "Comparrison Item " + idx.ToString() + " - 6GB Expandable Storage";
                    Products.Add(product);
                }
            }
        }
    }
}
