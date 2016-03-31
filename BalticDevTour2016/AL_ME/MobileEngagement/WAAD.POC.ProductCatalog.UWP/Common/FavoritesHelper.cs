using System;
using System.Collections.Generic;
using Windows.Storage;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public class FavoritesHelper
    {
        internal static List<string> DefaultFavorites = new List<string>();

        public static List<string> GetFavorites()
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Constants.Settings.FavoriteProductIds))
                {
                    string productIds = (ApplicationData.Current.LocalSettings.Values[Constants.Settings.FavoriteProductIds] as string)??"";
                    return productIds.DeserializeStringList();
                }
                else
                {
                    //No Favorites - Add Defaults?
                    if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(Constants.Settings.HasInitializedFavorites))
                    {
                        SaveFavorites(DefaultFavorites);
                        ApplicationData.Current.LocalSettings.Values.Add(Constants.Settings.HasInitializedFavorites, "Yes");
                        return DefaultFavorites;
                    }
                }
            }
            catch
            {
            }
            return new List<string>();
        }

        public static bool IsFavorite(string productId)
        {

            return (GetFavorites().Contains(productId ?? ""));
        }

        public static void RemoveFavorite(string productId)
        {
            if ((productId ?? "") == "") return;
            var favorites = GetFavorites();
            if (favorites.Contains(productId))
            {
                favorites.Remove(productId);
                SaveFavorites(favorites);
            }
        }

        public static void AddFavorite(string productId)
        {
            if ((productId ?? "") == "") return;
            var favorites = GetFavorites();
            if (!favorites.Contains(productId))
            {
                favorites.Add(productId);
                SaveFavorites(favorites);
            }
        }

        private static bool SaveFavorites(List<string> favorites)
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Constants.Settings.FavoriteProductIds))
                    ApplicationData.Current.LocalSettings.Values.Remove(Constants.Settings.FavoriteProductIds);
                ApplicationData.Current.LocalSettings.Values.Add(Constants.Settings.FavoriteProductIds, favorites.SerializeStringList());
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    

    }
}
