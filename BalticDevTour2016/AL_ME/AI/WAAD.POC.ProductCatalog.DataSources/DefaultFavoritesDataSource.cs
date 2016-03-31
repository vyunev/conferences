using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAAD.POC.ProductCatalog.DataSources
{
    public class DefaultFavoritesDataSource
    {
        private ILocalStoreDataService<string> _storageService;

        private IList<string> _productIds;

        public DefaultFavoritesDataSource(ILocalStoreDataService<string> storageService)
        {
            storageService.SourcePath = Constants.DefaultFavoritesFile;
            _storageService = storageService;
        }

        public async Task<IList<string>> GetAllAsync()
        {
            return _productIds ?? (_productIds = (await _storageService.ReadDataAsync()).ToList());
        }
    }
}
