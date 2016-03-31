namespace WAAD.POC.ProductCatalog.UWP.DisplayItems
{
    public class ProductListFilterItem
    {
        public string GroupId { get; set; }
        public string Title { get; }
        public bool IsAllItem { get; }
        public string FieldName { get; }

        public ProductListFilterItem(string title, string fieldName, bool isAllItem)
        {
            Title = title;
            IsAllItem = isAllItem;
            FieldName = fieldName;
        }
    }
}