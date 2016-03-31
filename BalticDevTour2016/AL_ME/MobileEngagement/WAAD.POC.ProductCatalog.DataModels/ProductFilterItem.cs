using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WAAD.POC.ProductCatalog.DataModels
{
    /// <summary>
    /// View model describing one of the filters available for viewing search results.
    /// </summary>
    public sealed class ProductFilterItem : INotifyPropertyChanged
    {
        private String _name;
        private List<string> _filters;


        public ProductFilterItem(String name, List<string> filters)
        {
            FilterName = name;
            FilterValues = filters;
        }

        public String FilterName
        {
            get { return _name; }
            set { if (SetProperty(ref _name, value)) OnPropertyChanged("FilterName"); }
        }

        public List<string> FilterValues
        {
            get { return _filters; }
            set { if (SetProperty(ref _filters, value)) OnPropertyChanged("FilterValues"); }
        }

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

 


}
