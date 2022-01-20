using MailContainerTest.Types;

namespace MailContainerTest.Data
{
    public interface IMailContainerDataStore
    {
        MailContainer GetMailContainer(string mailContainerNumber);

        void UpdateMailContainer(MailContainer mailContainer);

        bool AppropiateDataStore(string dataStore);
    }
}
