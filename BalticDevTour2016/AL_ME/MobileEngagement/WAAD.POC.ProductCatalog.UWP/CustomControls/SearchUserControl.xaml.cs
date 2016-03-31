using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using WAAD.POC.ProductCatalog.DataModels;
using WAAD.POC.ProductCatalog.DataSources;
using WAAD.POC.ProductCatalog.UWP.View;

namespace WAAD.POC.ProductCatalog.UWP.CustomControls
{
    public sealed partial class SearchUserControl 
    {

        public event EventHandler ForceClose;

        ObservableCollection<Product> _currentSuggestions;

        public SearchUserControl()
        {
            InitializeComponent();
            _currentSuggestions = new ObservableCollection<Product>();
            txtSearch.ItemsSource = _currentSuggestions;
            txtSearch.QuerySubmitted += OnQuerySubmitted;
            txtSearch.SuggestionChosen += TxtSearch_SuggestionChosen;
            txtSearch.TextChanged += TxtSearch_TextChanged;
        }


        /// <summary>
        /// Populates SearchBox with Suggestions when user enters text
        /// </summary>
        /// <param name="sender">The Xaml SearchBox</param>
        /// <param name="args">Event when user changes text in SearchBox</param>
        private async void TxtSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string text = txtSearch.Text ?? "";
                if (text == "")
                    _currentSuggestions.Clear();
                else
                {
                    _currentSuggestions.Clear();
                    foreach (var prd in (await DataManager.ProductDataSource.SearchProductsAsync(text)).Take(10))
                    {
                        _currentSuggestions.Add(prd);
                    }
                }
            }
        }

        private void TxtSearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is Product)
            {
                string productId = (args.SelectedItem as Product).Id;
                txtSearch.ItemsSource = null;
                AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(ViewProductPage), Parameter = productId });
            }
        }


        /// <summary>
        /// This method is called when query submitted in SearchBox
        /// </summary>
        /// <param name="sender">The Xaml SearchBox</param>
        /// <param name="args">Event when user submits query</param>
        private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
            }
            else if ((args.QueryText ?? "") != "")
            {
                AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(SearchResultsPage), Parameter = args.QueryText });
            }
            else
                return;

            txtSearch.Text = "";
            ForceClose?.Invoke(this, new EventArgs());
        }

        ///// <summary>
        ///// Populates SearchBox with Suggestions when user enters text
        ///// </summary>
        ///// <param name="sender">The Xaml SearchBox</param>
        ///// <param name="args">Event when user changes text in SearchBox</param>
        ////private async void SearchBoxEventsSuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        ////{
        ////    string queryText = args.QueryText;
        ////    SearchSuggestionCollection suggestionCollection = args.Request.SearchSuggestionCollection;
        ////    if (suggestionList==null)
        ////        suggestionList = await DataManager.ProductDataSource.GetAllProductNames();
        ////    foreach (string suggestion in suggestionList)
        ////    {
        ////        if (suggestion.ToUpper().Contains(queryText.ToUpper()))
        ////            suggestionCollection.AppendQuerySuggestion(suggestion);                
        ////    }
        ////}

        ///// <summary>
        ///// This method is called when query submitted in SearchBox
        ///// </summary>
        ///// <param name="sender">The Xaml SearchBox</param>
        ///// <param name="args">Event when user submits query</param>
        //private void SearchBoxEventsQuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        //{
        //    if (IsInlineSearch)
        //    {
        //        if (SearchRequested != null)
        //            SearchRequested.Invoke(this, new SearchRequestedEventArgs(args.QueryText));
        //    }
        //    else
        //        AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(View.SearchResultsPage), Parameter = args.QueryText });
        //}

        //public static readonly DependencyProperty IsInlineSearchProperty =
        //    DependencyProperty.Register("IsInlineSearch", typeof(bool), typeof(SearchUserControl), new PropertyMetadata(false));

        //public bool IsInlineSearch
        //{
        //    get { return (bool)GetValue(IsInlineSearchProperty); }
        //    set { SetValue(IsInlineSearchProperty, value); }
        //}

        //public string QueryText
        //{
        //    get {
        //        return (string)GetValue(QueryTextProperty);
        //    }
        //    set
        //    {
        //        SetValue(QueryTextProperty, value);
        //        txtSearch.Text = value ?? "";
        //    }
        //}

        //// Using a DependencyProperty as the backing store for QueryText.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty QueryTextProperty =
        //    DependencyProperty.Register("QueryText", typeof(string), typeof(SearchUserControl), new PropertyMetadata(""));


        //private void txtSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        //{
        //    if (IsInlineSearch)
        //    {
        //        if (SearchRequested != null)
        //            SearchRequested.Invoke(this, new SearchRequestedEventArgs(args.QueryText));
        //    }
        //    else
        //        AppShell.Current.NavCommand.Execute(new NavType() { Type = typeof(View.SearchResultsPage), Parameter = args.QueryText });
        //}
    }

    //public class SearchRequestedEventArgs : EventArgs
    //{
    //    public string SearchPhrase { get; set; }
    //    public SearchRequestedEventArgs(string searchPhrase) : base()
    //    {
    //        SearchPhrase = (searchPhrase ?? "").Trim();
    //    }
    //}
}
