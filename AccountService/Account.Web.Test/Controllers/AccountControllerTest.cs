using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Account.Business.Services.Interfaces;
using Account.Dto.WebDtos;
using Account.Web.Controllers;
using Account.Web.Validations.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Account.Web.Test.Controllers
{
    [TestFixture]
    public class PromocionesControllerTest
    {
        private Mock<IAccountService> _accountService;
        private Mock<IAccountValidation> _accountValidation;
        private Mock<ILogger<AccountController>> _logger;
        private CancellationToken _cancellationToken;

        private RegisterRequestDto _validRequest;

        private AccountController _accountController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _accountService = new Mock<IAccountService>();
            _accountValidation= new Mock<IAccountValidation>();
            _logger = new Mock<ILogger<AccountController>>();
            _cancellationToken = new CancellationToken();

            InitializeFakeVariables();

            _accountController = new AccountController(_accountService.Object, _accountValidation.Object, _logger.Object);
        }

        //[Test]
        public async Task RegisterAsync_WithValidDto_ReturnsOk()
        {
            //Arrange
            //_accountService.Setup(x => x.RegisterAsync(_validRequest, "", _cancellationToken)).Returns(Task.FromResult());

            //Act
            var result = await _accountController.RegisterAsync(_validRequest, _cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<CreatedAtRouteResult>());
            //Assert.That((result as CreatedAtRouteResult)?.Value, Is.TypeOf<List<Promocion>>());
        }

        [Test]
        public async Task ConfirmEmailAsync_WithRightToken_ReturnsOK()
        {
            //Arrange
            var token = "test";
            var email = "test@hotmail.com";

            //_accountService.Setup(x => x.RegisterAsync(_validRequest, "", _cancellationToken)).Returns(Task.FromResult(new IdentityResult().Succeeded));

            //Act
            var result = await _accountController.ConfirmEmailAsync(email, token, _cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<CreatedAtRouteResult>());
            //Assert.That((result as CreatedAtRouteResult)?.Value, Is.TypeOf<List<Promocion>>());
        }


        private void InitializeFakeVariables()
        {
            _validRequest = new RegisterRequestDto
            {
                Email = "test@hotmail.com",
                Password = "Password123!",
            };
        }


        [Test]
        public async Task GetAsync_Returns_Promotion()
        {
            //Arrange
            var guid = Guid.NewGuid();
            Mock.Arrange(() => _promocionesServices.GetByIdAsync(guid, _cancellationToken)).Returns(Task.FromResult(new Promocion()));

            //Act
            var result = await _promocionesController.GetAsync(guid, _cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<Promocion>());
        }

        [Test]
        public async Task GetActivePromotionsAsync_Returns_List()
        {
            //Arrange
            var list = new List<Promocion>
            {
                new Promocion()
            };
            Mock.Arrange(() => _promocionesServices.GetActivePromotionsAsync(Arg.AnyDateTime, _cancellationToken)).Returns(Task.FromResult(list));

            //Act
            var result = await _promocionesController.GetActivePromotionsAsync(new DateTime(1993, 4, 18), _cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<List<Promocion>>());
        }

        [Test]
        public async Task GetSalePromotionAsync_Returns_List()
        {
            //Arrange
            var dto = new GetSalePromotionDto();
            IEnumerable<Promocion> list = new List<Promocion>
            {
                new Promocion()
            };
            Mock.Arrange(() => _promocionesServices.GetActiveForSalePromotionsAsync(dto, _cancellationToken)).Returns(Task.FromResult(list));

            //Act
            var result = await _promocionesController.GetSalePromotionAsync(dto, _cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<IEnumerable<Promocion>>());
        }

        [Test]
        public async Task CreateAsync_Returns_List()
        {
            //Arrange
            var dto = new PromotionDto
            {
                Banks = new List<string> { "Galicia" },
                PaymentMethods = new List<string> { "EFECTIVO" },
                Categories = new List<string> { "Audio" },
                DiscountPorcentage = 40,
                StartDate = new DateTime(2020),
                EndDate = new DateTime(2021)
            };

            Mock.Arrange(() => _promocionesServices.CreateAsync(dto, _cancellationToken)).Returns(Task.FromResult(new Promocion()));

            //Act
            var result = await _promocionesController.CreateAsync(dto, _cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<Guid>());
        }

        [Test]
        public async Task UpdateAsync_Returns_List()
        {
            //Arrange
            var dto = new PromotionDto
            {
                Banks = new List<string> { "Galicia" },
                PaymentMethods = new List<string> { "EFECTIVO" },
                Categories = new List<string> { "Audio" },
                DiscountPorcentage = 40,
                StartDate = new DateTime(2020),
                EndDate = new DateTime(2021)
            };
            var guid = Guid.NewGuid();
            Mock.Arrange(() => _promocionesServices.UpdateAsync(guid, dto, _cancellationToken)).Returns(Task.FromResult(new Promocion()));

            //Act
            var result = await _promocionesController.UpdateAsync(guid, dto, _cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<Promocion>());
        }

        [Test]
        public async Task DeleteAsync_Returns_List()
        {
            //Arrange
            var dto = new PromotionDto();
            var guid = Guid.NewGuid();
            Mock.Arrange(() => _promocionesServices.DeleteAsync(guid, _cancellationToken)).Returns(Task.CompletedTask);

            //Act
            var result = await _promocionesController.DeleteAsync(guid, _cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkResult>());
        }
    }
}
