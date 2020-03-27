using System;
using ModelGraph.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ModelGraph.Repository
{
    public class RepositoryStorageFile : IRepository
    {
        StorageFile _storageFile;

        public RepositoryStorageFile(StorageFile storageFile)
        {
            _storageFile = storageFile;
        }

        #region FullName  =====================================================
        public string FullName => _storageFile.Path;
        public string Name
        {
            get
            {
                var name = _storageFile.Name;
                var index = name.LastIndexOf(".");
                if (index < 0) return name;
                return name.Substring(0, index);
            }
        }
        #endregion

        #region Read  =========================================================
        public async void Read(Chef chef)
        {
            try
            {
                using (var stream = await _storageFile.OpenAsync(FileAccessMode.Read))
                {
                    using (DataReader r = new DataReader(stream))
                    {
                        r.ByteOrder = ByteOrder.LittleEndian;
                        UInt64 size = stream.Size;
                        if (size < UInt32.MaxValue)
                        {
                            var byteCount = await r.LoadAsync((UInt32)size);
                            chef.Deserialize(r);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                chef.AddRepositorReadError(ex.Message);
            }
        }
        #endregion

        #region Write  ========================================================
        public async void Write(Chef chef)
        {
            try
            {
                using (var tran = await _storageFile.OpenTransactedWriteAsync())
                {
                    using (var w = new DataWriter(tran.Stream))
                    {
                        w.ByteOrder = ByteOrder.LittleEndian;
                        chef.Serialize(w);
                        tran.Stream.Size = await w.StoreAsync(); // reset stream size to override the file
                        await tran.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                chef.AddRepositorWriteError(ex.Message);
            }
        }
        #endregion

    }
}
