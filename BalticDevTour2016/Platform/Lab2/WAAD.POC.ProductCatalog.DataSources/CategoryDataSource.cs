using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAAD.POC.ProductCatalog.DataModels;

namespace WAAD.POC.ProductCatalog.DataSources
{
    public class CategoryDataSource
    {

        private ILocalStoreDataService<ProductCategory> _storageService;

        private IList<ProductCategory> _categories;

        public CategoryDataSource(ILocalStoreDataService<ProductCategory> storageService)
        {
            storageService.SourcePath = Constants.CategoryFile;
            _storageService = storageService;
        }

        public async Task<string> GetSubCategoryNameAsync(string subCategoryId)
        {
            var allCategories = await GetAllAsync();
            var subCategory = allCategories
                                    .SelectMany(category => category.SubCategoryItems)
                                    .SingleOrDefault(
                                        subCat =>
                                            subCat.Id.ToLowerInvariant()
                                                        .Equals(subCategoryId.ToLowerInvariant()));

            return subCategory != null ? subCategory.Name : null;
        }

        public async Task<string> GetCategoryNameAsync(string categoryId)
        {
            var allCategories = await GetAllAsync();
            var subCategory = allCategories

                                    .SingleOrDefault(
                                        subCat =>
                                            subCat.Id.ToLowerInvariant()
                                                        .Equals(categoryId.ToLowerInvariant()));

            return subCategory != null ? subCategory.Name : null;
        }
        public async Task<IList<ProductCategory>> GetSubCategoriesAsync(string categoryId)
        {
            var allCategories = await GetAllAsync();
            return allCategories
                    .Where(
                    category =>
                        category.Id.ToLowerInvariant().Equals(categoryId.ToLowerInvariant()))
                    .ToList();
        }

        public async Task<IList<ProductCategory>> GetAllAsync()
        {
            return _categories ?? (_categories = (await _storageService.ReadDataAsync()).ToList());
        }

        public async Task<ProductCategory> GetById(string id)
        {
            var allCategories = await GetAllAsync();

            return allCategories
                    .SingleOrDefault(
                        category =>
                            category.Id.ToLowerInvariant().Equals(id.ToLowerInvariant()));
        }

        public async Task<ProductCategory> GetParentCategory(string subCategoryId)
        {
            var allCategories = await GetAllAsync();
            var category = allCategories.FirstOrDefault(o => o.SubCategoryItems.Where(item => item.Id == subCategoryId).Any());
            return category;
        }
    }
}
