using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.UWP.Common;

namespace WAAD.POC.ProductCatalog.UWP.DisplayItems
{
    public class ProductCategoryDisplayItem
    {
        public ProductCategory Category { get; }
        public Command<ProductSubCategory> SubCategoryClickedCommand {get;}

        public ProductCategoryDisplayItem(ProductCategory category,
            Command<ProductSubCategory> subCategoryClickedCommand)
        {
            Category = category;
            SubCategoryClickedCommand = subCategoryClickedCommand;
        }
    }
}