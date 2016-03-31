using System.Collections.Generic;
using System.Linq;
using WAAD.POC.ProductCatalog.DataModels;

namespace WAAD.POC.ProductCatalog.UWP.Data
{
    public class DesignTimeDataService
    {

        public static IList<ProductCategory> GenerateProductCategories()
        {
            var results = new List<ProductCategory>
            {
                new ProductCategory() {Id = "1", Name = "Audio", SubCategoryItems = GenerateAudioSubCategories()},
                new ProductCategory() {Id = "2", Name = "Cameras", SubCategoryItems = GenerateCameraSubCategories()},
                new ProductCategory()
                {
                    Id = "3",
                    Name = "Home Appliances",
                    SubCategoryItems = GenerateAppliancesSubCategories()
                }
            };
            return results.OrderBy(o => o.Name).ToList();
        }


        public static List<Product> GenerateFavoriteProducts(int maxCount, string brand="Fabrikam")
        {
            var results = new List<Product>();
            for (int x = 0; x < maxCount; x++)
                results.Add(GenerateProduct("Product " + x.ToString(), brand));
            return results;
        }
        



        public static List<ProductSubCategory> GenerateAudioSubCategories()
        {
            var results = new List<ProductSubCategory>
            {
                new ProductSubCategory()
                {
                    Id = "1",
                    Name = "Headphones",
                    ImagePath = "ms-appx://Data/ProductCategoryImages/4020101.png"
                },
                new ProductSubCategory()
                {
                    Id = "2",
                    Name = "Speakers",
                    ImagePath = "ms-appx://Data/ProductCategoryImages/4020101.png"
                },
                new ProductSubCategory()
                {
                    Id = "3",
                    Name = "Car Audio",
                    ImagePath = "ms-appx://Data/ProductCategoryImages/4020101.png"
                },
                new ProductSubCategory()
                {
                    Id = "4",
                    Name = "MP3",
                    ImagePath = "ms-appx://Data/ProductCategoryImages/4020101.png"
                }
            };
            return results;

        }

        public static List<ProductSubCategory> GenerateCameraSubCategories()
        {
            var results = new List<ProductSubCategory>
            {
                new ProductSubCategory() {Id = "1", Name = "Camcorders"},
                new ProductSubCategory() {Id = "2", Name = "Digital Cameras"},
                new ProductSubCategory() {Id = "3", Name = "Camera Accessories"},
                new ProductSubCategory() {Id = "4", Name = "Digital SLR Cameras"}
            };
            return results;
        }

        public static List<ProductSubCategory> GenerateAppliancesSubCategories()
        {
            var results = new List<ProductSubCategory>
            {
                new ProductSubCategory() {Id = "1", Name = "Air conditioners"},
                new ProductSubCategory() {Id = "2", Name = "Microwaves"},
                new ProductSubCategory() {Id = "3", Name = "Washers and dryers"}
            };
            return results;
        }

        public static Product GenerateProduct(string name="Fabrikam SLR Camera X150", string brand="Fabrikam")
        {
            var product = new Product()
            {
                Brand = "Fabrikam",
                Category = "2",
                Description = "Retro style SLR with two-mode live view. The secondary sensor in the viewfinder provides live view with fast phase-detection AF. A great choice if you plan to do most of your shooting in live view.",
                GroupNumber = "5637145873",
                Id = "5637146578",
                ImagePath = "Data/ProductImages/2020506.png",
                Name = "Fabrikam SLR Camera X150",
                Price = 639,
                ProductColor = "Green",
                SubCategory = "11",
                ProductSpecifications = new List<ProductSpecification>(
                      new ProductSpecification[]
                      {
                            new ProductSpecification() {Name="Size", Value="11.4" },
                            new ProductSpecification() {Name="Height", Value="11.4" },
                            new ProductSpecification() {Name="Width", Value="11.4" },
                            new ProductSpecification() {Name="Battery Life", Value="11.4" },
                            new ProductSpecification() {Name="Weight", Value="11.4" }
                      }
                  )


            };

            
            return product;

        }
    }
}
