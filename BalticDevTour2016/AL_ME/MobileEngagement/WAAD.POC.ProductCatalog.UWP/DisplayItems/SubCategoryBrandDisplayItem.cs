using System.Collections.Generic;
using WAAD.POC.ProductCatalog.DataModels;

namespace WAAD.POC.ProductCatalog.UWP.DisplayItems
{
    public class SubCategoryBrandDisplayItem
    {
        public List<Product> Items { get; }
        public string Name { get; }
        public string Id { get; }

        public SubCategoryBrandDisplayItem(string id, string name, List<Product> items)
        {
            Id = id;
            Name = name;
            Items = items;
        }
    }
}