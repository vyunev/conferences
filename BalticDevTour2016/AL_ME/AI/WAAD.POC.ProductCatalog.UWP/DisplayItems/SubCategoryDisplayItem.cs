using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.View;
using WAAD.POC.ProductCatalog.UWP.ViewModel;

namespace WAAD.POC.ProductCatalog.UWP.DisplayItems
{
    public class SubCategoryDisplayItem : Bindable
    {
        private ProductListFilterViewModel _filterViewModel;
        internal List<ProductListFilterItem> CurrentFilters { get; set; }

        public string Title => Name;
        public string Name { get; set; }
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<Product> AllProducts { get; set; }
        public List<Product> FilteredProducts { get; set; }
        private ObservableCollection<object> _selectedItems = new ObservableCollection<object>();

        public bool HasResults => (FilteredProducts?.Count > 0);
        public bool HasFiltersEnabled => (CurrentFilters?.Count > 0);
        public bool IsItemsSelected { get; private set; }
        public bool IsMultiSelectEnabled {get; set;}
        public bool IsMultiSelectDisabled => (!IsMultiSelectEnabled);

        public List<SubCategoryBrandDisplayItem> AllBrands
        {
            get
            {
                List<SubCategoryBrandDisplayItem> results = new List<SubCategoryBrandDisplayItem>();
                if (FilteredProducts!=null)
                    foreach (var brand in FilteredProducts.Select(o => o.Brand).Distinct().OrderBy(o => o))
                        results.Add(new SubCategoryBrandDisplayItem(brand, brand, (FilteredProducts.Where(d => d.Brand == brand).ToList())));
                return results;
            }
        }

#region "Product Filtering"

        public List<ProductListFilterItem> SelectedFilters
        {
            get
            {
                if (CurrentFilters == null)
                    return new List<ProductListFilterItem>();
                return
                    CurrentFilters.Select(
                        item =>
                            new ProductListFilterItem(item.Title, item.FieldName, false)
                            {
                                GroupId = item.GroupId
                            }).ToList();                
            }
        }

        public void ResetFilteredProducts()
        {
            FilteredProducts = new List<Product>(AllProducts);
            CurrentFilters = new List<ProductListFilterItem>();
            OnPropertyChanged("SelectedFilters");
            OnPropertyChanged("AllBrands");
            OnPropertyChanged("HasResults");
            OnPropertyChanged("HasFiltersEnabled");
        }

        public void SetFilteredProducts(IEnumerable<Product> filteredProducts, IEnumerable<ProductListFilterItem> selectedFilters)
        {
            FilteredProducts = new List<Product>(filteredProducts);
            CurrentFilters = new List<ProductListFilterItem>(selectedFilters);
            OnPropertyChanged("SelectedFilters");
            OnPropertyChanged("AllBrands");
            OnPropertyChanged("HasResults");
            OnPropertyChanged("HasFiltersEnabled");

        }

#endregion

#region "Relay Commands"

        private Command _cancelMultiSelectCommand;
        private Command _enableMultiSelectCommand;
        private Command _showFiltersCommand;
        private Command _removeAllFiltersCommand;
        private Command<ProductListFilterItem> _removeFilterCommand;
        private Command<ObservableCollection<object>> _compareClickedCommand;
        private Command<Product> _itemClickedCommand;        

        public Command<Product> ItemClickedCommand
        {
            get
            {
                return _itemClickedCommand
                       ?? (_itemClickedCommand = new Command<Product>(
                           val =>
                           {
                               AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(ViewProductPage), Parameter = val.Id });
                           }));
            }
        }

       
        public Command CancelMultiSelectCommand
        {
            get
            {
                return _cancelMultiSelectCommand
                       ?? (_cancelMultiSelectCommand = new Command(
                           () =>
                           {
                               IsMultiSelectEnabled = false;
                               OnPropertyChanged("IsMultiSelectEnabled");
                               OnPropertyChanged("IsMultiSelectDisabled");
                           }));
            }
        }


        public Command ShowFiltersCommand
        {
            get
            {
                return _showFiltersCommand
                       ?? (_showFiltersCommand = new Command(
                           () =>
                           {
                               if (_filterViewModel == null)
                               {
                                   _filterViewModel = new ProductListFilterViewModel();
                                   _filterViewModel.InitializeFilterGroupsFromProductList(this);
                               }
                               FlyoutHelper.ShowProductFilterFlyout(this, _filterViewModel);
                           }));
            }
        }

        public Command<ProductListFilterItem> RemoveFilterCommand
        {
            get
            {
                return _removeFilterCommand
                       ?? (_removeFilterCommand = new Command<ProductListFilterItem>(
                           item =>
                           {
                               _filterViewModel?.ClearFilter(item.GroupId);
                           }));
            }
        }


        public Command RemoveAllFiltersCommand
        {
            get
            {
                return _removeAllFiltersCommand
                       ?? (_removeAllFiltersCommand = new Command(
                           () =>
                           {
                               if (_filterViewModel!=null)
                                   _filterViewModel.ClearAllFilters();
                           }));
            }
        }


    
        public Command EnableMultiSelectCommand
        {
            get
            {
                return _enableMultiSelectCommand
                       ?? (_enableMultiSelectCommand = new Command(
                           () =>
                           {
                               IsMultiSelectEnabled = true;
                               OnPropertyChanged("IsMultiSelectEnabled");
                               OnPropertyChanged("IsMultiSelectDisabled");
                           }));
            }
        }

        public Command<ObservableCollection<object>> CompareClickedCommand
        {
            get
            {
                return _compareClickedCommand
                       ?? (_compareClickedCommand = new Command<ObservableCollection<object>>(
                           val =>
                           {
                               if (val?.Count > 0)
                               {
                                   List<string> idsSelected = val.Select(o => (o as Product).Id).ToList<string>();
                                   AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(CompareProductsPage), Parameter = idsSelected.SerializeStringList() });
                               }
                           }));
            }
        }

#endregion

#region "Selected Items"

        public ObservableCollection<object> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;                              
            }
        }

        private void _selectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_selectedItems != null)
                IsItemsSelected = _selectedItems.Count > 0;
            else
                IsItemsSelected = false;
            OnPropertyChanged("IsItemsSelected");
        }

#endregion


        public SubCategoryDisplayItem(string categoryId, string categoryName, ProductSubCategory subCategory)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Id = subCategory.Id;
            Name = subCategory.Name;
            AllProducts = new List<Product>();
            FilteredProducts = new List<Product>();
            CurrentFilters = new List<ProductListFilterItem>();
            _selectedItems.CollectionChanged += _selectedItems_CollectionChanged;
            IsMultiSelectEnabled = false;
        }


    }
}