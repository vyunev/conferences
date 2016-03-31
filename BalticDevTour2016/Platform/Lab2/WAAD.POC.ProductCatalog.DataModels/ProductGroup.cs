using System.Collections.Generic;

namespace WAAD.POC.ProductCatalog.DataModels
{
    public class ProductGroup
    {
        public string Title { get; set; }

        public List<Product> Items { get; set; }

        public int UnFilteredItemsCount { get; set; }
    }

    public class ProductGroup<T> : List<T>
    {
        public string Title { get; set; }

        public int UnFilteredItemsCount { get; set; }

        public ProductGroup(string title, IEnumerable<T> items, int unFilteredItemsCount)
            : base(items)
        {
            Title = title;
            UnFilteredItemsCount = unFilteredItemsCount;
        }
    }
}
