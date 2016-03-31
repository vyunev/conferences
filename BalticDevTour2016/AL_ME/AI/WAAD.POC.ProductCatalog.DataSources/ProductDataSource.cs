using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WAAD.POC.ProductCatalog.DataModels;

namespace WAAD.POC.ProductCatalog.DataSources
{
    public class ProductDataSource 
    {
        private ILocalStoreDataService<Product> _storageService;

        private IList<Product> _products;

        public ProductDataSource(ILocalStoreDataService<Product> storageService)
        {
            storageService.SourcePath = Constants.ProductFile;
            _storageService = storageService;
        }

        private void CleanProductSpecifications(IList<Product> products)
        {
            Regex rg = new Regex("([a-z])([A-Z\\(])");
            foreach (var prd in products)
            {
                if (prd.ProductSpecifications!=null)
                    foreach (var specification in prd.ProductSpecifications)
                    {
                        specification.Name = rg.Replace(specification.Name, "$1 $2");
                        //clean the Name..
                        //string name = specification.Name;
                        //name = rg.Replace(name, "$1 $2");
                        //specification.Name = name;
                    }
            }
        }

        public async Task<IList<Product>> GetAllAsync()
        {
            if (_products != null) return _products;
            var products = (await _storageService.ReadDataAsync()).ToList();
            CleanProductSpecifications(products);
            _products = products;
            return _products;
        }

        /// <summary>
        /// Gets the product details.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public async Task<Product> GetDetailsAsync(string productId)
        {

            var allProducts = await GetAllAsync();
            var matches = allProducts.Where(item => item.Id.Equals(productId));
            if (matches != null && matches.Count() == 1) return matches.First();
            return null;
        }

        /// <summary>
        /// Gets the products by sub category id.
        /// </summary>
        /// <param name="subCategoryId">The sub category id.</param>
        /// <returns></returns>
        public async Task<List<Product>> GetProductsBySubCategoryIdAsync(string subCategoryId)
        {
            var allProducts = await GetAllAsync();
            var matches = allProducts.Where(item => item.SubCategory == subCategoryId).ToList();
            return matches;
        }

        /// <summary>
        /// Gets some random products for live tile to flip through.
        /// </summary>
        /// <param name="count">Number of random products</param>
        /// <returns>List of products</returns>
        public async Task<List<Product>> GetRandomProductsAsync(int count)
        {
            var allProducts = await GetAllAsync();
            var matches = allProducts.OrderBy(item => Guid.NewGuid()).Take(count).ToList();
            return matches;
        }

        public async Task<List<Product>> GetRandomProductsByCategoryAsync(int count, string categoryId)
        {
            var allProducts = await GetAllAsync();
            var matches = allProducts.Where(item=>item.Category==categoryId).OrderBy(item => Guid.NewGuid()).Take(count).ToList();
            return matches;
        }

        public async Task<List<Product>> GetRandomProductsBySubCategoryAsync(int count, string subcategoryId)
        {
            var allProducts = await GetAllAsync();
            var matches = allProducts.Where(item => item.SubCategory == subcategoryId).OrderBy(item => Guid.NewGuid()).Take(count).ToList();
            return matches;
        }

        /// <summary>
        /// This method searches the products by search text. The search text can be a part of the 
        /// Product name or Product description
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>Search results</returns>
        public async Task<List<Product>> SearchProductsAsync(string searchText)
        {
            var allProducts = await GetAllAsync();
            return
                allProducts.Where(
                    item =>
                    item.Name.ToUpper().Contains(searchText.ToUpper()) ||
                    item.Description.ToUpper().Contains(searchText.ToUpper())).ToList();
        }

        /// <summary>
        /// This method gets all the Products using the product IDs supplied.
        /// </summary>
        /// <returns>List of Product objects</returns>
        public async Task<List<Product>> GetProductsByProductIdsAsync(List<string> productIDs)
        {
            var allProducts = await GetAllAsync();
            var matches = allProducts.Where(item => productIDs.Contains(item.Id)).ToList();
            return matches;
        }

        public async Task<List<string>> GetAllProductNames()
        {
            var allProducts = await GetAllAsync();
            var matches = from item in allProducts
                          select item.Name;
            return matches.ToList();
        }
    }
}
