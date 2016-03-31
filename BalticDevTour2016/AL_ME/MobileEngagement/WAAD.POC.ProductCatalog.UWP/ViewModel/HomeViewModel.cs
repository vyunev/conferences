using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.DataSources;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.Data;
using WAAD.POC.ProductCatalog.UWP.DisplayItems;



namespace WAAD.POC.ProductCatalog.UWP.ViewModel
{

    public class HomeViewModel : BaseViewModel
    {

        List<Product> _favorites;
        List<ProductCategory> _mainCategories;

        public HomeViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
            }
        }

        public override async Task InitializeViewModel(string param)
        {
            if (HasInitialized)
            {
                //If we have initialized the data already, update the favorites only to reflect any changes made.
                FavoritesHelper.DefaultFavorites = (await DataManager.DefaultFavoritesDataSource.GetAllAsync()).ToList();
                _favorites = (await DataManager.ProductDataSource.GetProductsByProductIdsAsync(FavoritesHelper.GetFavorites())).ToList();
                OnPropertyChanged("FavoriteProducts");
                return;
            }
            
            //Get list of Favorite Products
            FavoritesHelper.DefaultFavorites = (await DataManager.DefaultFavoritesDataSource.GetAllAsync()).ToList();
            _favorites = (await DataManager.ProductDataSource.GetProductsByProductIdsAsync(FavoritesHelper.GetFavorites())).ToList();

            //Get list of Top Level Categories
            _mainCategories = (await DataManager.CategoryDataSource.GetAllAsync()).ToList();

            
        }

        public IList<ProductCategoryDisplayItem> AllCategories
        {
            get
            {
                // Get reference to list of Product Categories
                // If design time then generate a fake list
                IList<ProductCategory> results;
                results = DesignMode.DesignModeEnabled ? DesignTimeDataService.GenerateProductCategories() : _mainCategories;

                //Return a List of ProductCategoryDisplayItem instances for the View.
                return results.Select(item => new ProductCategoryDisplayItem(item, SubCategoryClickedCommand )).ToList();
            }
        }



        public IList<Product> FavoriteProducts
        {
            get
            {
                // Return Current list of Favorite Products
                // If Design Time then generate some.
                if (DesignMode.DesignModeEnabled)
                    return DesignTimeDataService.GenerateFavoriteProducts(7);
                else
                    return _favorites;
            }
        }


    }

}
