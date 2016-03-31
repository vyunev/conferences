using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.DisplayItems;

namespace WAAD.POC.ProductCatalog.UWP.ViewModel
{
    public class ProductListFilterViewModel : Bindable
    {
        private ObservableCollection<ProductListFilterGroupDisplayItem> _filterGroups = new ObservableCollection<ProductListFilterGroupDisplayItem>();
        private SubCategoryDisplayItem _subCategoryDisplayItem = null;

        public ProductListFilterViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                //create some fake data for XAML Designer
                _filterGroups.Clear();

                _filterGroups.Add(new ProductListFilterGroupDisplayItem(this, "Price Range", "Price", new List<ProductListFilterItem>()
                {
                    new ProductListRangeFilterItem("Any Price", "Price", true, Decimal.MinValue, Decimal.MaxValue),
                    new ProductListRangeFilterItem("$100 - $200", "Price", true, Decimal.MinValue, Decimal.MaxValue),
                    new ProductListRangeFilterItem("$300 - $400", "Price", true, Decimal.MinValue, Decimal.MaxValue),
                    new ProductListRangeFilterItem("$500 - $600", "Price", true, Decimal.MinValue, Decimal.MaxValue),
                    new ProductListRangeFilterItem("$600 - $700", "Price", true, Decimal.MinValue, Decimal.MaxValue)
                }));


                _filterGroups.Add(new ProductListFilterGroupDisplayItem(this, "MegaPixels", "MegaPixels", new List<ProductListFilterItem>()
                {
                    new ProductListFilterItem("Any", "MegaPixels", true),
                    new ProductListFilterItem("2.2", "MegaPixels", false),
                    new ProductListFilterItem("4", "MegaPixels", false),
                    new ProductListFilterItem("8", "MegaPixels", false),
                    new ProductListFilterItem("16", "MegaPixels", false)
                }));

                _filterGroups.Add(new ProductListFilterGroupDisplayItem(this, "Memory", "Memory", new List<ProductListFilterItem>()
                {
                    new ProductListFilterItem("Any", "Memory", true),
                    new ProductListFilterItem("2.2", "Memory", false),
                    new ProductListFilterItem("4", "Memory", false),
                    new ProductListFilterItem("8", "Memory", false),
                    new ProductListFilterItem("16", "Memory", false)
                }));

                _filterGroups.Add(new ProductListFilterGroupDisplayItem(this, "Size", "size", new List<ProductListFilterItem>()
                {
                    new ProductListFilterItem("Any", "Size", true),
                    new ProductListFilterItem("2.2", "Size", false),
                    new ProductListFilterItem("4", "Size", false),
                    new ProductListFilterItem("8", "Size", false)
                }));

                _filterGroups.Add(new ProductListFilterGroupDisplayItem(this, "FocalPoint", "FocalPoint", new List<ProductListFilterItem>()
                {
                    new ProductListFilterItem("Any", "FocalPoint", true),
                    new ProductListFilterItem("2.2", "FocalPoint", false),
                    new ProductListFilterItem("4.3", "FocalPoint", false),
                    new ProductListFilterItem("8.3", "FocalPoint", false),
                    new ProductListFilterItem("12.1", "FocalPoint", false)
                }));


            }
        }

        // Command for Removing All Filters.
        private Command _removeAllFiltersCommand;
        public Command RemoveAllFiltersCommand
        {
            get
            {
                return _removeAllFiltersCommand
                       ?? (_removeAllFiltersCommand = new Command(ClearAllFilters));
            }
        }

        public void ClearAllFilters()
        {
            
            if (_filterGroups != null)
            {
                // Set All Filtergroups to default ('All')
                bool clearedFilter = false;
                foreach (var fltr in _filterGroups.Where(o=>o.IsGroupFiltered))
                {
                    fltr.ClearFilter();
                    clearedFilter = true;
                }
                if (clearedFilter)
                {
                    //If we cleared a filter update parent datasets
                    NotifyFiltersChanged();
                }
            }
        }

        public void ClearFilter(string uniqueGroupId)
        {
            if (_filterGroups != null)
            {
                // Try to find the matching Filter Group.
                var filterToClear = _filterGroups.FirstOrDefault(o => o.UniqueGroupId == uniqueGroupId);
                if (filterToClear != null)
                {
                    // Remove the filter for that filtergroup (set filter to 'All')
                    filterToClear.ClearFilter();

                    // Update parent dataset with new changes / rebuild filtered product list.
                    NotifyFiltersChanged();
                }
            }
        }

        public void NotifyFiltersChanged()
        {
            
            try
            {
                if (_subCategoryDisplayItem.AllProducts != null)
                {
                    // Check if any filters have been enabled
                    var hasFilters = FilterGroups.Any(fg => fg.IsGroupFiltered);

                    if (hasFilters)
                    {
                        //Filter the list of Product Items from our Filter Groups
                        List<ProductListFilterItem> selectedFilterItems;
                        var filteredProducts = GetFilteredProducts(_subCategoryDisplayItem.AllProducts, FilterGroups.ToList(), out selectedFilterItems);

                        // Update the parent SubCategory with new filtered product list + enabled filters.
                        _subCategoryDisplayItem.SetFilteredProducts(filteredProducts, selectedFilterItems);
                    }
                    else
                    {
                        // Notify the parent Subcategory to reset products back to full unfiltered list
                        _subCategoryDisplayItem.ResetFilteredProducts();
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        private static List<Product> GetFilteredProducts(List<Product> allProducts, List<ProductListFilterGroupDisplayItem> groups, out List<ProductListFilterItem> selectedFilterItems)
        {
            //Initialize our Product list with all the products + create collection to hold filters. 
            List<Product> filteredProducts = new List<Product>(allProducts);
            selectedFilterItems = new List<ProductListFilterItem>();


            //Check if there is a price filter enabled
            var priceFilter = groups.FirstOrDefault(o => (o.IsGroupFiltered) && (o.FieldName == "Price"));
            if (priceFilter != null)
                if (priceFilter.SelectedFilterItem is ProductListRangeFilterItem)
                {
                    // Filter product list to match min/max price range
                    var range = (priceFilter.SelectedFilterItem as ProductListRangeFilterItem);
                    filteredProducts =
                        filteredProducts.Where(item => (item.Price >= range.Min) && (item.Price < range.Max))
                            .ToList();
                    // Add filter to the selected filters collection
                    selectedFilterItems.Add(range);
                }

            // Check if other (non price) filters are enabled + apply to valid products
            var otherFilters = groups.Where(o => (o.IsGroupFiltered) && (o.FieldName != "Price")).ToList();
            if (otherFilters.Count > 0)
            {
                foreach (var filter in otherFilters)
                {
                    // Get the ProductSpecification value we want to find.
                    string checkVal = filter.SelectedFilterItem?.Title ?? "";

                    // Filter our prodcuts list to those with matching Specification/Value
                    if (checkVal != "")
                    {
                        filteredProducts =
                            filteredProducts.Where(
                                item =>
                                    item.ProductSpecifications
                                        .Where(
                                            spec =>
                                                (spec.Name == filter.FieldName) && (spec.Value == checkVal))
                                        .Any()).ToList();

                        // Add the filter to the selected filters collection
                        selectedFilterItems.Add(filter.SelectedFilterItem);
                    }
                }
            }
            return filteredProducts;
        }

        public ObservableCollection<ProductListFilterGroupDisplayItem> FilterGroups
        {
            get { return _filterGroups; }
            set
            {
                _filterGroups = value;
                OnPropertyChanged("FilterGroups");
            }
        }

        //Initialize the ViewModel Filter Groups via referenced SubCategoryDisplayItem
        public void InitializeFilterGroupsFromProductList(SubCategoryDisplayItem subCategoryDisplayItem)
        {
            _subCategoryDisplayItem = subCategoryDisplayItem;
            
            // Build our list of Filter Groups
            _filterGroups.Clear();
            if (_subCategoryDisplayItem?.AllProducts != null)
                if (_subCategoryDisplayItem.AllProducts.Any())
                {
                    foreach (var groupItem in BuildFilterGroups(this, _subCategoryDisplayItem.AllProducts.ToList()))
                    {
                        // Add EventHandler so we can react to filter changes (and notify parent page)
                        groupItem.FilterUpdated += GroupItem_FilterUpdated;

                        _filterGroups.Add(groupItem);
                    }
                }
        }

        private void GroupItem_FilterUpdated(object sender, EventArgs e)
        {
            NotifyFiltersChanged();
        }

        private static ObservableCollection<ProductListFilterGroupDisplayItem> BuildFilterGroups(ProductListFilterViewModel parent, List<Product> products)
        {
            // Create Filter Groups Collection
            ObservableCollection<ProductListFilterGroupDisplayItem> newFilterGroups = new ObservableCollection
                <ProductListFilterGroupDisplayItem>
            {
                // Initialize with Filter for the Price Range
                new ProductListFilterGroupDisplayItem(parent, "Price Range", "Price",
                    GetPriceRanges(products, true).ToList<ProductListFilterItem>())
            };

            // Determine what other filter groups are required by
            // scanning for ProductSpecifications which 
            // have AllowFiltering set to true.
            var dynamicFilterHeaders = products.SelectMany(x => x.ProductSpecifications ?? new List<ProductSpecification>())
                                        .Where(y => y.AllowFiltering == true)
                                        .Select(x => x.Name).Distinct().ToArray();



            foreach (var dynamicFilterHeader in dynamicFilterHeaders)
            {
                // Get list of filter values for new filter group
                var filterValues = products.SelectMany(x => x.ProductSpecifications)
                                        .Where(y => (y.AllowFiltering == true) && (y.Name == dynamicFilterHeader))
                                        .Select(x => x.Value).Distinct().ToList();

                // Create list of filter items 
                // initialize with an 'Any' option.
                var items = new List<ProductListFilterItem>
                {
                    new ProductListFilterItem("Any " + dynamicFilterHeader, dynamicFilterHeader, true)
                };
                //add the filter items from our list of values
                foreach (var item in filterValues)
                    items.Add(new ProductListFilterItem(item, dynamicFilterHeader, false));

                //add filter group 
                newFilterGroups.Add(
                    new ProductListFilterGroupDisplayItem(parent, dynamicFilterHeader, dynamicFilterHeader, items)
                    );
            }

            return newFilterGroups;
        }


        //todo - move to constants?
        private const string MoneySymbol = "$";

        private static List<ProductListRangeFilterItem> GetPriceRanges(List<Product> products, bool addAllItem)
        {
            List<ProductListRangeFilterItem> priceRanges = new List<ProductListRangeFilterItem>();

            var minPrice = products.Min(x => x.Price);
            var maxPrice = products.Max(x => x.Price);

            var flooredMin = (minPrice - minPrice % 10);
            var ceiledMax = (maxPrice + (10 - maxPrice % 10));

            var maxOptions = 4;

            var difference = ceiledMax - flooredMin;
            var step = Math.Floor(difference / maxOptions);
            var range = new List<string>();

            if (addAllItem)
                priceRanges.Add(new ProductListRangeFilterItem("Any Price", "Price", true, Decimal.MinValue, Decimal.MaxValue));

            for (int i = 0; i < maxOptions - 1; i++)
                priceRanges.Add(GetPriceRangeItem((flooredMin + i * step), (flooredMin + (i + 1) * step)));

            //do final item.
            priceRanges.Add(GetPriceRangeItem((flooredMin + (maxOptions - 1) * step), ceiledMax));
            return priceRanges;
        }

        private static ProductListRangeFilterItem GetPriceRangeItem(decimal minRangePrice, decimal maxRangePrice)
        {
            string rangeTitle = MoneySymbol + minRangePrice.ToString("F0") + "-" + MoneySymbol +
                                maxRangePrice.ToString("F0");
            return new ProductListRangeFilterItem(rangeTitle, "Price", false, minRangePrice, maxRangePrice);
        }
    }

  
}

