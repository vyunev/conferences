using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.StartScreen;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.DataSources;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.Data;

namespace WAAD.POC.ProductCatalog.UWP.ViewModel
{
    public class ViewProductViewModel : BaseViewModel
    {
        public Product ProductDetails { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public List<Product> RelatedProducts { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsPinned { get; set; }

        private Command _addToFavoritesCommand ;
        private Command _removeFromFavoritesCommand ;

        public Command AddToFavoritesCommand
        {
            get
            {
                return _addToFavoritesCommand
                       ?? (_addToFavoritesCommand = new Command(
                           () =>
                           {
                               if (ProductDetails == null) return;
                               FavoritesHelper.AddFavorite(ProductDetails.Id);
                               IsFavorite = FavoritesHelper.IsFavorite(ProductDetails.Id);
                               OnPropertyChanged("IsFavorite");
                           }));
            }
        }

        public Command RemoveFromFavoritesCommand
        {
            get
            {
                return _removeFromFavoritesCommand
                       ?? (_removeFromFavoritesCommand = new Command(
                           () =>
                           {
                               if (ProductDetails == null) return;
                               FavoritesHelper.RemoveFavorite(ProductDetails.Id);
                               IsFavorite = FavoritesHelper.IsFavorite(ProductDetails.Id);
                               OnPropertyChanged("IsFavorite");
                           }));
            }
        }



        public override async Task InitializeViewModel(string param)
        {
            if (HasInitialized) return;
            if (param != "")
            {
                var currentProduct = await DataManager.ProductDataSource.GetDetailsAsync(param);
                if (currentProduct != null)
                {
                    ProductDetails = currentProduct;
                    ProductName = currentProduct.Name;
                    List<Product> products = ((await DataManager.ProductDataSource.GetProductsBySubCategoryIdAsync(currentProduct.SubCategory))
                        .Where(item => item.Id != currentProduct.Id)
                        .Where(item => item.GroupNumber.Equals(currentProduct.GroupNumber))).ToList();
                    CategoryName = (await DataManager.CategoryDataSource.GetCategoryNameAsync(currentProduct.Category));
                    SubCategoryName = (await DataManager.CategoryDataSource.GetSubCategoryNameAsync(currentProduct.SubCategory));
                    RelatedProducts = products;
                    IsFavorite = FavoritesHelper.IsFavorite(currentProduct.Id);
                    CheckPinnedState();
                }
            }
        }

        public void CheckPinnedState()
        {
            try
            {
                if (ProductDetails!=null)
                {
                    IsPinned = SecondaryTile.Exists(ProductDetails.Id);
                    OnPropertyChanged("IsPinned");
                    return;
                }
            }
            catch
            {
            }
            IsPinned = false;
            OnPropertyChanged("IsPinned");
        }

        //Manually Set the Pinned State While System Creates It.
        public void SetPinnedState(bool isPinned)
        {
            try
            {
                IsPinned = isPinned;
                OnPropertyChanged("IsPinned");
                return;
            }
            catch
            {
            }

            IsPinned = false;
            OnPropertyChanged("IsPinned");
        }

        public ViewProductViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                ProductDetails = DesignTimeDataService.GenerateProduct();
                ProductName = ProductDetails.Name;
                CategoryName = "Cameras";
                SubCategoryName = "SLR Cameras";
                RelatedProducts = DesignTimeDataService.GenerateFavoriteProducts(5);
            }
            IsPinned = false;
            IsFavorite = false;


        }
    }
}
