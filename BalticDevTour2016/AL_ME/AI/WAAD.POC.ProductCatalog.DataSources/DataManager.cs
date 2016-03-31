using System;
using WAAD.POC.ProductCatalog.DataModels;

namespace WAAD.POC.ProductCatalog.DataSources
{
    public static class DataManager
    {
        private static bool _isInitialized;
        private static Exception _notInitializedException = new Exception("DataManager needs to initialized before accessing its members.");

        public static void Initialize(
            ILocalStoreDataService<ProductCategory> categoryStorageService,
            ILocalStoreDataService<Product> productStorageService, 
            ILocalStoreDataService<string> defaultFavoritesStorageService
            ) 
        {
            CategoryDataSource = new CategoryDataSource(categoryStorageService);
            ProductDataSource = new ProductDataSource(productStorageService);
            DefaultFavoritesDataSource = new DefaultFavoritesDataSource(defaultFavoritesStorageService);
            _isInitialized = true;
        }


        private static CategoryDataSource _categoryDataSource;
        public static CategoryDataSource CategoryDataSource
        {
            get
            {
                if (_isInitialized)
                    return _categoryDataSource;
                throw _notInitializedException;
            }
            set
            {
                _categoryDataSource = value;
            }
        }

        private static ProductDataSource _productDataSource;
        public static ProductDataSource ProductDataSource
        {
            get
            {
                if (_isInitialized)
                    return _productDataSource;
                throw _notInitializedException;
            }
            set
            {
                _productDataSource = value;
            }
        }

        private static DefaultFavoritesDataSource _defaultFavoritesDataSource;
        public static DefaultFavoritesDataSource DefaultFavoritesDataSource
        {
            get
            {
                if (_isInitialized)
                    return _defaultFavoritesDataSource;
                throw _notInitializedException;
            }
            set
            {
                _defaultFavoritesDataSource = value;
            }
        }



        #region "Legacy Methods for Win8.1/WP8.0 versions"

        ////Legacy Method for Win8/WP client.
        //public static async Task CopyOfflineDataFiles()
        //{

        //}

        ////Legacy Method for Win8/WP client.
        //public async static Task Initialize(
        //    ILocalStoreDataService<ProductCategory> categoryStorageService,
        //    ILocalStoreDataService<Product> productStorageService
        //    )
        //{
        //    Initialize(categoryStorageService, productStorageService, null);
        //}


        #endregion

    }
}
