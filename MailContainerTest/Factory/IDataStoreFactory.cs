using MailContainerTest.Data;

namespace MailContainerTest.Factory
{
    public interface IDataStoreFactory
    {
        IMailContainerDataStore GetContainerDataStore(string dataStore);
    }
}
