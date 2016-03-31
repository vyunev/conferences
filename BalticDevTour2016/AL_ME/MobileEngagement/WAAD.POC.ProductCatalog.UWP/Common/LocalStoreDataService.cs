using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using WAAD.POC.ProductCatalog.DataSources;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public class LocalStoreDataService<T> : ILocalStoreDataService<T>
        where T:class
    {
        private string _sourcePath;

        public string SourcePath
        {
            get { return _sourcePath; }
             set { _sourcePath = value; }
        }

        public virtual async Task<IEnumerable<T>> ReadDataAsync()
        {
            try
            {
                var dataFolder = await Package.Current.InstalledLocation.GetFolderAsync(Constants.DataFilesFolder);
                StorageFile sessionFile = await dataFolder.GetFileAsync(SourcePath);

                using (IRandomAccessStreamWithContentType sessionInputStream = await sessionFile.OpenReadAsync())
                {
                    var sessionSerializer = new DataContractSerializer(typeof(T[]));
                    var restoredData = (T[])sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());
                    return restoredData.ToList();
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public virtual async Task<T> ReadSingleDataAsync()
        {
            try
            {
                var dataFolder = await Package.Current.InstalledLocation.GetFolderAsync(Constants.DataFilesFolder);
                StorageFile sessionFile = await dataFolder.GetFileAsync(SourcePath);

                using (IRandomAccessStreamWithContentType sessionInputStream = await sessionFile.OpenReadAsync())
                {
                    var sessionSerializer = new DataContractSerializer(typeof(T));
                    var restoredData = (T)sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());
                    return restoredData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>> ReadDataFromLocalStorageAsync()
        {
            try
            {
                var dataFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(Constants.DataFilesFolder);
                StorageFile sessionFile = await dataFolder.GetFileAsync(SourcePath);

                using (IRandomAccessStreamWithContentType sessionInputStream = await sessionFile.OpenReadAsync())
                {
                    var sessionSerializer = new DataContractSerializer(typeof(T[]));
                    var restoredData = (T[])sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());
                    return restoredData.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual async Task<bool> WriteDataAsync(IList<T> data)
        {
            try
            {
                StorageFolder store = await ApplicationData.Current.LocalFolder.GetFolderAsync(Constants.DataFilesFolder);
                StorageFile sessionFile = await store.CreateFileAsync(SourcePath, CreationCollisionOption.ReplaceExisting);

                using (IRandomAccessStream sessionRandomAccess = await sessionFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0))
                    {
                        var sessionSerializer = new DataContractSerializer(typeof(T[]), new Type[] { typeof(T[]) });
                        sessionSerializer.WriteObject(sessionOutputStream.AsStreamForWrite(), data.ToArray());
                        await sessionOutputStream.FlushAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
