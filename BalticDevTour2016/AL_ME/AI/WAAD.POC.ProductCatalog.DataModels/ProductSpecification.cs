namespace WAAD.POC.ProductCatalog.DataModels
{
    public class ProductSpecification
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool? AllowComparision { get; set; }

        public bool? AllowFiltering { get; set; }
    }
}
