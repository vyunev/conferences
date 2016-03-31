namespace WAAD.POC.ProductCatalog.UWP.DisplayItems
{
    public class ProductListRangeFilterItem : ProductListFilterItem
    {
        public decimal Min { get; }
        public decimal Max { get; }

        public ProductListRangeFilterItem(string title, string fieldName, bool isAllItem, decimal min, decimal max) 
            : base(title, fieldName, isAllItem)
        {
            Min = min;
            Max = max;
        }
    }
}