using System.Collections.Generic;
using System.Threading.Tasks;

namespace WAAD.POC.ProductCatalog.DataSources
{
    public interface ILocalStoreDataService<T>
         where T : class
    {
        string SourcePath { get; set; }

        Task<IEnumerable<T>> ReadDataAsync();

        Task<IEnumerable<T>> ReadDataFromLocalStorageAsync();

        Task<T> ReadSingleDataAsync();

        Task<bool> WriteDataAsync(IList<T> data);
    }
}
