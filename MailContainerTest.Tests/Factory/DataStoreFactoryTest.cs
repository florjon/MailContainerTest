using MailContainerTest.Data;
using MailContainerTest.Factory;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MailContainerTest.Tests.Factory
{
    public class DataStoreFactoryTest
    {
        private DataStoreFactory _dataStoreFactory;
        private Mock<IMailContainerDataStore> _mailContainerDataStore1;
        private Mock<IMailContainerDataStore> _mailContainerDataStore2;

        public DataStoreFactoryTest()
        {
            _mailContainerDataStore1 = new Mock<IMailContainerDataStore>();
            _mailContainerDataStore2 = new Mock<IMailContainerDataStore>();
            _dataStoreFactory = new DataStoreFactory(new List<IMailContainerDataStore> { _mailContainerDataStore1.Object, _mailContainerDataStore2.Object });
        }

        [Fact]
        public void ManifestServiceFactory_Matching_ManifestService_Return_Highest()
        {
            //arrange
            _mailContainerDataStore1.Setup(f => f.AppropiateDataStore(It.IsAny<string>())).Returns(true);
            _mailContainerDataStore2.Setup(f => f.AppropiateDataStore(It.IsAny<string>())).Returns(false);

            //act
            var response = _dataStoreFactory.GetContainerDataStore("backup");

            //assert
            Assert.Equal(response, _mailContainerDataStore1.Object);

            _mailContainerDataStore1.Verify(f => f.AppropiateDataStore(It.IsAny<string>()), Times.AtLeastOnce);
            _mailContainerDataStore2.Verify(f => f.AppropiateDataStore(It.IsAny<string>()), Times.Never);
        }
    }
}
