using MailContainerTest.Data;
using MailContainerTest.Factory;
using MailContainerTest.Services;
using MailContainerTest.Types;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MailContainerTest.Tests.Services
{
    public class MailTransferServiceTest
    {
        private Mock<IMailContainerDataStore> _containerDataStore;
        private Mock<IDataStoreFactory> _dataStoreFactory;
        private MailTransferService _mailTransferService;

        public MailTransferServiceTest()
        {
            _containerDataStore = new Mock<IMailContainerDataStore>();
            _dataStoreFactory = new Mock<IDataStoreFactory>();
            _mailTransferService = new MailTransferService(_containerDataStore.Object, _dataStoreFactory.Object);
        }

        [Fact]
        public async Task MailTransferService_GetMailContainer_Return_False()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>()));

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest());

            //assert
            _dataStoreFactory.Verify(f => f.GetContainerDataStore(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.GetMailContainer(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Never);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task MailTransferService_MailType_StandardLetter_Return_True()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>())).Returns(new MailContainer() { AllowedMailType = AllowedMailType.StandardLetter });
            _containerDataStore.Setup(f => f.UpdateMailContainer(It.IsAny<MailContainer>()));

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest() { MailType = MailType.StandardLetter, SourceMailContainerNumber = It.IsAny<string>(), DestinationMailContainerNumber = It.IsAny<string>(), NumberOfMailItems = 1, TransferDate = System.DateTime.Today });

            //assert


        }

        [Fact]
        public async Task MailTransferService_MailType_StandardLetter_Return_False()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>())).Returns(new MailContainer() { AllowedMailType = AllowedMailType.LargeLetter });

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest() { MailType = MailType.StandardLetter, SourceMailContainerNumber = It.IsAny<string>(), DestinationMailContainerNumber = It.IsAny<string>(), NumberOfMailItems = 1, TransferDate = System.DateTime.Today });

            //assert
            _dataStoreFactory.Verify(f => f.GetContainerDataStore(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.GetMailContainer(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Never);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task MailTransferService_MailType_SmallParcel_MailContainerStatus_Operational_Return_True()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>())).Returns(new MailContainer() { AllowedMailType = AllowedMailType.SmallParcel, Status = MailContainerStatus.Operational });
            _containerDataStore.Setup(f => f.UpdateMailContainer(It.IsAny<MailContainer>()));

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest() { MailType = MailType.SmallParcel, SourceMailContainerNumber = It.IsAny<string>(), DestinationMailContainerNumber = It.IsAny<string>(), NumberOfMailItems = 1, TransferDate = System.DateTime.Today });

            //assert
            _dataStoreFactory.Verify(f => f.GetContainerDataStore(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.GetMailContainer(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Once);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task MailTransferService_MailType_SmallParcel_MailContainerStatus_OutOfService_Return_False()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>())).Returns(new MailContainer() { AllowedMailType = AllowedMailType.SmallParcel, Status = MailContainerStatus.OutOfService });

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest() { MailType = MailType.SmallParcel, SourceMailContainerNumber = It.IsAny<string>(), DestinationMailContainerNumber = It.IsAny<string>(), NumberOfMailItems = 1, TransferDate = System.DateTime.Today });

            //assert
            _dataStoreFactory.Verify(f => f.GetContainerDataStore(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.GetMailContainer(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Never);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task MailTransferService_MailType_SmallParcel_MailContainerStatus_OutOfService_AllowedMailType_LargeLetter_Return_False()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>())).Returns(new MailContainer() { AllowedMailType = AllowedMailType.LargeLetter, Status = MailContainerStatus.OutOfService });

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest() { MailType = MailType.SmallParcel, SourceMailContainerNumber = It.IsAny<string>(), DestinationMailContainerNumber = It.IsAny<string>(), NumberOfMailItems = 1, TransferDate = System.DateTime.Today });

            //assert
            _dataStoreFactory.Verify(f => f.GetContainerDataStore(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.GetMailContainer(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Never);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task MailTransferService_MailType_LargeLetter_Capacity_More_Then_NumberOfMailItems_Return_True()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>())).Returns(new MailContainer() { AllowedMailType = AllowedMailType.LargeLetter, Capacity = 1, MailContainerNumber = It.IsAny<string>() });
            _containerDataStore.Setup(f => f.UpdateMailContainer(It.IsAny<MailContainer>()));

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest() { MailType = MailType.LargeLetter, SourceMailContainerNumber = It.IsAny<string>(), DestinationMailContainerNumber = It.IsAny<string>(), NumberOfMailItems = 1, TransferDate = System.DateTime.Today });

            //assert
            _dataStoreFactory.Verify(f => f.GetContainerDataStore(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.GetMailContainer(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Once);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task MailTransferService_MailType_LargeLetter_Capacity_Less_Then_NumberOfMailItems_Return_False()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>())).Returns(new MailContainer() { AllowedMailType = AllowedMailType.LargeLetter, Capacity = 0, MailContainerNumber = It.IsAny<string>() });

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest() { MailType = MailType.LargeLetter, SourceMailContainerNumber = It.IsAny<string>(), DestinationMailContainerNumber = It.IsAny<string>(), NumberOfMailItems = 1, TransferDate = System.DateTime.Today });

            //assert
            _dataStoreFactory.Verify(f => f.GetContainerDataStore(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.GetMailContainer(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Never);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task MailTransferService_MailType_LargeLetter_Capacity_Less_Then_NumberOfMailItems_AllowedMailType_StandardLetter_Return_False()
        {
            //arrange
            _dataStoreFactory.Setup(f => f.GetContainerDataStore(It.IsAny<string>())).Returns(_containerDataStore.Object);
            _containerDataStore.Setup(f => f.GetMailContainer(It.IsAny<string>())).Returns(new MailContainer() { AllowedMailType = AllowedMailType.StandardLetter, Capacity = 0, MailContainerNumber = It.IsAny<string>() });

            //act
            var response = _mailTransferService.MakeMailTransfer(new MakeMailTransferRequest() { MailType = MailType.LargeLetter, SourceMailContainerNumber = It.IsAny<string>(), DestinationMailContainerNumber = It.IsAny<string>(), NumberOfMailItems = 1, TransferDate = System.DateTime.Today });

            //assert
            _dataStoreFactory.Verify(f => f.GetContainerDataStore(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.GetMailContainer(It.IsAny<string>()), Times.Once);
            _containerDataStore.Verify(f => f.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Never);
            Assert.False(response.Success);
        }
    }
}
