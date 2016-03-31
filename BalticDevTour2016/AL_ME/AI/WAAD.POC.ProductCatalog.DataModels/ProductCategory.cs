using System.Collections.Generic;

namespace WAAD.POC.ProductCatalog.DataModels
{
    public class ProductCategory
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public List<ProductSubCategory> SubCategoryItems { get; set; }
    }
}
