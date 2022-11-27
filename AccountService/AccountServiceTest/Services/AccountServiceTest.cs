using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace AccountServiceTest.Services
{
    public class AccountServiceTest
    {
        //private IAccountRepository _repositoryMock;
        //private IAccountValidation _validationMock;
        //private CancellationToken _cancellationToken;
        
        //private IAccountDomainService _service;

        //[OneTimeSetUp]
        //public void Setup()
        //{
        //    _repositoryMock = new Mock<IAccountRepository>(MockBehavior.Strict).Object;
        //    _validationMock = new Mock<IAccountValidation>(MockBehavior.Strict).Object;
        //    _cancellationToken = new CancellationToken();

        //    _service = new AccountDomainService(_repositoryMock, _validationMock);
        //}

        //[Test]
        //public async Task CreateAccountAsync_Return_CorrectAccount()
        //{
        //    // Arrange
        //    var createDto = new CreateAccountDto { 
        //        UserName = "UserNameTest"
        //    };

        //    // Act
        //    var result = await _service.CreateAccountAsync(createDto, _cancellationToken);

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //}
    }
}