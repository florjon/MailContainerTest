using MailContainerTest.Data;
using System.Collections.Generic;
using System.Linq;

namespace MailContainerTest.Factory
{
    public class DataStoreFactory : IDataStoreFactory
    {
        private readonly IEnumerable<IMailContainerDataStore> _containerDataStore;

        public DataStoreFactory(IEnumerable<IMailContainerDataStore> containerDataStore)
        {
            _containerDataStore = containerDataStore;
        }

        public IMailContainerDataStore GetContainerDataStore(string dataStore)
        {
            return _containerDataStore.FirstOrDefault(f => f.AppropiateDataStore(dataStore));
        }
    }
}
