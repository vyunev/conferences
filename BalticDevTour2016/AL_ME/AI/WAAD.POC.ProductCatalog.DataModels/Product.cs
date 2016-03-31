using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WAAD.POC.ProductCatalog.DataModels
{
    public class Product
    {
        private string _descriptionField;

        public string Id { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string Description
        {
            get { return _descriptionField; }
            set
            {
                var description = value == null
                                      ? string.Empty
                                      : value.Split('\n')
                                             .Aggregate(string.Empty,
                                                        (current, s) =>
                                                        string.IsNullOrEmpty(current)
                                                            ? s.Trim()
                                                            : string.Format(CultureInfo.CurrentCulture, "{0}\n{1}",
                                                                            current,
                                                                            s.Trim()));
                _descriptionField = value;

            }
        }

        public decimal Price { get; set; }

        public List<ProductSpecification> ProductSpecifications { get; set; }

        public string GroupNumber { get; set; }

        public string Brand { get; set; }

        public string ProductColor { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, ProductColor);
        }
    }
}
