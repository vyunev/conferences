using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.DataSources;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.Data;
using WAAD.POC.ProductCatalog.UWP.DisplayItems;
using WAAD.POC.ProductCatalog.UWP.View;

//using WAAD.POC.ProductCatalog.UWP.Common;

namespace WAAD.POC.ProductCatalog.UWP.ViewModel
{



    public class ProductListingViewModel : BaseViewModel
    {
        public string PageName => CategoryName;
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SubCategoryDisplayItem> PivotItems { get; set; }

        public SubCategoryDisplayItem SelectedPivot { get; set; }
        public List<Product> FavoriteItems { get; set; }

        public override async Task InitializeViewModel(string param)
        {
            if (HasInitialized) return;
            ProductCategory category = await DataManager.CategoryDataSource.GetById(param);
            List<ProductSubCategory> subCategories = null;
            ProductSubCategory selectedSubCategory = null;

            if (category == null)
            {
                //Not a Category - must be a subcategory
                category = await DataManager.CategoryDataSource.GetParentCategory(param);
                if (category != null)
                {
                    subCategories = category.SubCategoryItems;
                    selectedSubCategory = subCategories.FirstOrDefault(item => item.Id == param);
                }
                else
                {
                    //error - id is not a category or subcategory
                    throw new Exception("The Parameter is not a Category or a SubCategory");
                }
            }
            else
            {
                //Populate the SubCategories + get the first SubCategory;
                subCategories = category.SubCategoryItems;
                selectedSubCategory = subCategories.FirstOrDefault();
            }

            CategoryId = category.Id;

            //Populate the SubCategory Display Items..
            ObservableCollection<SubCategoryDisplayItem> subCategoryDisplayItems = new ObservableCollection<SubCategoryDisplayItem>();
            SubCategoryDisplayItem selectedSubCategoryDisplayItem = null;
            foreach (var subCategory in subCategories)
            {
                var sc = new SubCategoryDisplayItem(category.Id, category.Name, subCategory);
                var prds = (await DataManager.ProductDataSource.GetProductsBySubCategoryIdAsync(subCategory.Id)).ToList();
                sc.AllProducts = new List<Product>(prds);
                sc.FilteredProducts = new List<Product>(prds);
                subCategoryDisplayItems.Add(sc);
                if (selectedSubCategory.Id == sc.Id)
                    selectedSubCategoryDisplayItem = sc;
            }

            CategoryName = category.Name;
            CategoryId = category.Id;
            SelectedPivot = selectedSubCategoryDisplayItem;
            PivotItems = subCategoryDisplayItems.ToList();
            
        }

        public ProductListingViewModel()
        {
            CategoryId = "1";
            CategoryName = "CategoryName";
            PivotItems = new List<SubCategoryDisplayItem>();

            if (DesignMode.DesignModeEnabled)
            {
                //Generate Fake Design Time Data.
                foreach (var subCategory in DesignTimeDataService.GenerateAudioSubCategories())
                {
                    var sc = new SubCategoryDisplayItem("1", "Audio", subCategory);
                    var prds = new List<Product>();
                    prds.AddRange(DesignTimeDataService.GenerateFavoriteProducts(9, "Fabrikam"));
                    prds.AddRange(DesignTimeDataService.GenerateFavoriteProducts(4, "Contoso"));
                    prds.AddRange(DesignTimeDataService.GenerateFavoriteProducts(7, "Proseware"));
                    sc.AllProducts = new List<Product>(prds);
                    sc.FilteredProducts = new List<Product>(prds);
                    sc.CurrentFilters.Add(new ProductListFilterItem("$222 - $333", "Price Range", false));
                    sc.CurrentFilters.Add(new ProductListFilterItem("32", "Megabytes", false));
                    sc.CurrentFilters.Add(new ProductListFilterItem("5x", "Optical Zoom", false));
                    if (PivotItems.Count == 0)
                    {
                        SelectedPivot = sc;
                    }
                    PivotItems.Add(sc);
                }
                FavoriteItems = DesignTimeDataService.GenerateFavoriteProducts(4);
                HasInitialized = true;
            }

        }

    }

}
