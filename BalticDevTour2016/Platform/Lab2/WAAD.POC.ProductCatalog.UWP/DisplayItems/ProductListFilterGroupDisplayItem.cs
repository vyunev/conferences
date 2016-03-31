using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.ViewModel;

namespace WAAD.POC.ProductCatalog.UWP.DisplayItems
{
    public class ProductListFilterGroupDisplayItem : Bindable
    {
        //Event Handler to Notify parent that a filter has been changed.
        public event EventHandler FilterUpdated;

        private ProductListFilterViewModel _parent;
        private ObservableCollection<ProductListFilterItem> _items;
        private ProductListFilterItem _selectedFilterItem;

        public ObservableCollection<ProductListFilterItem> Items => _items;
        public string Title { get; }
        public string FieldName { get; }
        public string UniqueGroupId { get; }


        public ProductListFilterItem SelectedFilterItem
        {
            get { return _selectedFilterItem; }
            set
            {           
                _selectedFilterItem = value;
                FilterUpdated?.Invoke(this, new EventArgs());
            }
        }

        public bool IsGroupFiltered
        {
            get
            {
                if (_selectedFilterItem!=null)
                    return (!_selectedFilterItem.IsAllItem);
                return false;
            }
        }


        public void ClearFilter(bool suppressChanges = true)
        {
            try
            {
                // Reset this Filter Group back to default ('All' filter).
                var allFilter = _items.FirstOrDefault(item => item.IsAllItem);
                _selectedFilterItem = allFilter;
                OnPropertyChanged("SelectedFilterItem");

                //Invoke Notification event on change.
                if (!suppressChanges)
                    FilterUpdated?.Invoke(this, new EventArgs());
            }
            catch (Exception)
            {
                //place to throw an exception
            }
        }



        public ProductListFilterGroupDisplayItem(ProductListFilterViewModel parent, string title, string fieldName, List<ProductListFilterItem> items)
        {
            //Create a UniqueID to identify this group of filters.
            UniqueGroupId = Guid.NewGuid().ToString();

            //Initialize Instance
            Title = title;
            FieldName = fieldName;
            _parent = parent;
            _items = new ObservableCollection<ProductListFilterItem>();
            _selectedFilterItem = null;

            //Populate the Filter Items
            if (items?.Count > 0)
            {
                foreach (var item in items)
                {
                    //Assign Filter Item to Group Id
                    item.GroupId = UniqueGroupId;
                    _items.Add(item);
                }
                _selectedFilterItem = items.FirstOrDefault();
            }

        }
    }
}