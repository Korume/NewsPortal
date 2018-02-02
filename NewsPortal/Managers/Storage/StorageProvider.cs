using System.Collections.Generic;
using NewsPortal.Interfaces;
using NewsPortal.Managers.LocalMemory;

namespace NewsPortal.Managers.Storage
{
    public enum MemoryMode { Database, LocalStorage };

    public class StorageProvider
    {
        static MemoryMode currentStorageMode;

        static Dictionary<MemoryMode, IStorage> storageManagers = new Dictionary<MemoryMode, IStorage>(2);

        static StorageProvider()
        {
            storageManagers.Add(MemoryMode.Database, new DataBaseManager());
            storageManagers.Add(MemoryMode.LocalStorage, new LocalMemoryManager());
        }

        public static IStorage GetStorage()
        {
            return storageManagers[currentStorageMode];
        }

        public static void SwitchStorage(MemoryMode value)
        {
            currentStorageMode = value;
        }

        public static MemoryMode ModeStorage
        {
            get
            {
                return currentStorageMode;
            }
        }
    }

}