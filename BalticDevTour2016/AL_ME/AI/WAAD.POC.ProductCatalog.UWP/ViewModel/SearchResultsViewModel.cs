using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.DataSources;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.Data;
using WAAD.POC.ProductCatalog.UWP.DisplayItems;

//using WAAD.POC.ProductCatalog.UWP.Common;

namespace WAAD.POC.ProductCatalog.UWP.ViewModel
{
    using Microsoft.ApplicationInsights;

    public class SearchResultsViewModel : BaseViewModel
    {
        public bool IsWindowsPhone { get; }
        public string PageName => IsInitializing ? "Initializing Search" : (IsWindowsPhone ? "SEARCH" : string.Format("Search for '{0}'", SearchPhrase??"(no phrase)"));
        public string SearchPhrase { get; set; }
        public ObservableCollection<SearchGroupDisplayItem> PivotItems { get; set; }
        public bool HasResults => (PivotItems?.Any() ?? false);

        public SearchGroupDisplayItem SelectedPivot { get; set; }

        public bool IsInitializing { get; set; }

        public string NoResultsText => ((SearchPhrase??"")=="") ? "No search phrase has been provided" : string.Format("Search for '{0}' returned no results", SearchPhrase ?? "-");

        public override async Task InitializeViewModel(string param)
        {

            if (HasInitialized) return;
            await PerformSearch(param ?? "");
        }

        private async Task PerformSearch(string param)
        {
            SearchPhrase = param ?? "";
            var allItems = ((param??"")=="") ? new List<Product>() : (await DataManager.ProductDataSource.SearchProductsAsync(param));


            var nbResults = allItems.Count;

            // Provide properties by which you can filter events:
            var properties = new Dictionary<string, string>
                                 {
                                     { "Search.Param", param },
                                     { "Search.Results", nbResults.ToString() }
                                 };

            var tc = new TelemetryClient();
            tc.TrackEvent("Search", properties);


            if (allItems?.Any() ?? false)
            {
                // Track nb search results
                tc.TrackMetric("Search result", nbResults , properties);

                // We want to track case of exact match
                if (nbResults == 1)
                {
                    var product = allItems.FirstOrDefault();
                    properties.Add("Product.Id", product.Id);
                    properties.Add("Product.Name", product.Name);
                    tc.TrackEvent("Exact Search", properties);
                }

                PivotItems.Clear();                
                PivotItems.Add(new SearchGroupDisplayItem("", "All", allItems, true, ItemClickedCommand));
                SelectedPivot = PivotItems[0];
                foreach (string categoryId in allItems.Select(item => item.Category).Distinct())
                {
                    string categoryName = await DataManager.CategoryDataSource.GetCategoryNameAsync(categoryId);
                    PivotItems.Add(new SearchGroupDisplayItem(categoryId, categoryName, allItems.Where(item => item.Category == categoryId).ToList(), false, ItemClickedCommand));
                }
            }
            else
            {
                PivotItems.Clear();
                SelectedPivot = null;
            }
            IsInitializing = false;
            OnPropertyChanged("IsInitializing");
            OnPropertyChanged("PivotItems");
            OnPropertyChanged("PageName");
            OnPropertyChanged("HasResults");
            OnPropertyChanged("SearchPhrase");
            OnPropertyChanged("NoResultsText");
            OnPropertyChanged("SearchPhrase");
        }


        public SearchResultsViewModel()
        {
            IsWindowsPhone = DeviceFamilyHelper.CurrentDeviceFamily() == DeviceFamily.Mobile;
            IsInitializing = true;
            SearchPhrase = "";
            PivotItems = new ObservableCollection<SearchGroupDisplayItem>();

            if (DesignMode.DesignModeEnabled)
            {
                PivotItems.Add(new SearchGroupDisplayItem("0", "All", DesignTimeDataService.GenerateFavoriteProducts(12), true, null));
                PivotItems.Add(new SearchGroupDisplayItem("1", "Audio", DesignTimeDataService.GenerateFavoriteProducts(8), false, null));
                PivotItems.Add(new SearchGroupDisplayItem("2", "Cameras", DesignTimeDataService.GenerateFavoriteProducts(6), false, null));
                PivotItems.Add(new SearchGroupDisplayItem("4", "Home Appliances", DesignTimeDataService.GenerateFavoriteProducts(3), false, null));
                SearchPhrase = "MYPHRASE";
                IsInitializing = false;
                HasInitialized = true;
            }
        }

        private Command<string> _searchInvokedCommand;
        public Command<string> SearchInvokedCommand
        {
            get
            {
                return _searchInvokedCommand
                       ?? (_searchInvokedCommand = new Command<string>(
                           async val =>
                           {
                               await PerformSearch(val);
                               //AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(ViewProductPage), Parameter = val.Id });
                           }));
            }
        }

    }

  
}
