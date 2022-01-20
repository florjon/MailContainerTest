using MailContainerTest.Data;
using MailContainerTest.Factory;
using MailContainerTest.Types;
using System.Configuration;

namespace MailContainerTest.Services
{
    public class MailTransferService : IMailTransferService
    {
        private IMailContainerDataStore _mailContainerDataStore;
        private IDataStoreFactory _dataStoreFactory;

        public MailTransferService(IMailContainerDataStore mailContainerDataStore, IDataStoreFactory dataStoreFactory)
        {
            _mailContainerDataStore = mailContainerDataStore;
            _dataStoreFactory = dataStoreFactory;
        }

        public MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request)
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            //created a factory which has the responsabilit to give the proper instance of the datastore based on the dataStoreType
            _mailContainerDataStore = _dataStoreFactory.GetContainerDataStore(dataStoreType);

            //get the MailContainer information based on the selected dataStore (backup or not)
            MailContainer mailContainer = _mailContainerDataStore.GetMailContainer(request.SourceMailContainerNumber);

            //this code was repeative withing the switch case. It is easier to read if it is extracted from there. 
            if (mailContainer == null)
                return new MakeMailTransferResult() { Success = false };

            MakeMailTransferResult result = GetMailTransferResult(request, mailContainer);

            if (result.Success)
            {
                mailContainer.Capacity -= request.NumberOfMailItems;

                //again, update the Mail container based on the dataStore selected before
                _mailContainerDataStore.UpdateMailContainer(mailContainer);
            }

            return result;
        }

        private static MakeMailTransferResult GetMailTransferResult(MakeMailTransferRequest request, MailContainer mailContainer)
        {
            var result = new MakeMailTransferResult() { Success = true };

            switch (request.MailType)
            {
                case MailType.StandardLetter:
                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.StandardLetter))
                    {
                        result.Success = false;
                    }
                    break;

                case MailType.LargeLetter:
                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.LargeLetter))
                    {
                        result.Success = false;
                    }
                    else if (mailContainer.Capacity < request.NumberOfMailItems)
                    {
                        result.Success = false;
                    }
                    break;

                case MailType.SmallParcel:
                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.SmallParcel))
                    {
                        result.Success = false;
                    }
                    else if (mailContainer.Status != MailContainerStatus.Operational)
                    {
                        result.Success = false;
                    }
                    break;
            }

            return result;
        }
    }
}