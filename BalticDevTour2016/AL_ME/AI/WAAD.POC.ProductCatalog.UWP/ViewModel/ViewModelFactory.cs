namespace WAAD.POC.ProductCatalog.UWP.ViewModel
{
    public enum ViewModelType
    {
        Home=0,
        SearchResults=1,
        AudioProductListing=2,
        CamerasProductListing = 3,
        AppliancesProductListing = 4,
        ViewProduct = 5,
        CompareProducts = 6
    }

    // This can be updated to plug into a ViewModelLocator if MVVM Libraries are used.
    public static class ViewModelFactory
    {
        public static BaseViewModel CreateViewModel(ViewModelType type)
        {
           switch(type)
           {
                case ViewModelType.Home:
                    return new HomeViewModel();
                case ViewModelType.SearchResults:
                    return new SearchResultsViewModel();
                case ViewModelType.AudioProductListing: 
                case ViewModelType.CamerasProductListing: 
                case ViewModelType.AppliancesProductListing:
                    return new ProductListingViewModel();
                case ViewModelType.ViewProduct:
                    return new ViewProductViewModel();
                case ViewModelType.CompareProducts:
                    return new CompareProductsViewModel();
            }
           return null;
        }
    }
}
