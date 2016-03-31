using System.Threading.Tasks;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.DataSources;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.View;

namespace WAAD.POC.ProductCatalog.UWP.ViewModel
{
    //This is the Base ViewModel class we will use throughout Project
    public abstract class BaseViewModel : Bindable
    {
        protected BaseViewModel()
        {
            HasInitialized = false;
        }

        //Command for navigating to a product page.
        private Command<Product> _itemClickedCommand;
        public Command<Product> ItemClickedCommand
        {
            get
            {
                return _itemClickedCommand
                       ?? (_itemClickedCommand = new Command<Product>(
                           val =>
                           {
                               AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(ViewProductPage), Parameter = val.Id });
                           }));
            }
        }

        //Command for navigating to a browse category page 
        // - passes in the subcategory as a parameter so page can navigate to that subcategorie's pivot by default.
        private Command<ProductSubCategory> _subCategoryClickedCommand;
        public Command<ProductSubCategory> SubCategoryClickedCommand
        {
            get
            {
                return _subCategoryClickedCommand
                       ?? (_subCategoryClickedCommand = new Command<ProductSubCategory>(
                           async val =>
                           {
                               var category = await DataManager.CategoryDataSource.GetParentCategory(val.Id);
                               if (category != null)
                               {
                                   var navType = new NavType() { Type = typeof(AudioProductListingPage), Parameter = val.Id };
                                   if (category.Id == "1")
                                       navType.Type = typeof(AudioProductListingPage);
                                   else if (category.Id == "2")
                                       navType.Type = typeof(CamerasProductListingPage);
                                   else if (category.Id == "4")
                                       navType.Type = typeof(AppliancesProductListingPage);

                                   AppShell.Current.NavCommand.Execute(navType);
                               }
                               
                           }));
            }
        }

        //Property to determine if View Model has been Initialized or not.
        public bool HasInitialized { get; set; }

        //Method to be implemented on each ViewModel 
        // - page parameter is passed in and method will load required data for page.
        public virtual async Task InitializeViewModel(string param)
        {

        }
        

    }
}
