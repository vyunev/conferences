using System.Collections.Generic;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.UWP.Common;

namespace WAAD.POC.ProductCatalog.UWP.DisplayItems
{
    public class SearchGroupDisplayItem : Bindable
    {

        public List<Product> Items { get; }
        public string Name { get; }
        public string Id { get; }
        public bool IsAllCategory { get; }
        public Command<Product> ItemClickedCommand { get; }

        public string Title => $"{Name} ({ResultsCount})";
        public int ResultsCount => Items?.Count ?? 0;

        public SearchGroupDisplayItem(string categoryId, string categoryName, List<Product> items, bool isAllCategory, Command<Product> itemClickedCommand)
        {
            Id = categoryId;
            Name = categoryName;
            Items = items;
            IsAllCategory = isAllCategory;
            ItemClickedCommand = itemClickedCommand;
        }

    }
}