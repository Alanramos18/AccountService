using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Account.Web.Controllers;
using NUnit.Framework;

namespace Account.Web.Test.Controllers
{
    [TestFixture]
    public class PromocionesControllerTest
    {
        private AccountController _accountController;
        private IPromocionesServices _promocionesServices;
        private CancellationToken _cancellationToken;

        [OneTimeSetUp]
        public void SetUp()
        {
            _promocionesServices = Mock.Create<IPromocionesServices>(Behavior.Strict);
            _cancellationToken = new CancellationToken();

            _promocionesController = new PromocionesController(_promocionesServices);
        }

        [Test]
        public async Task GetListAsync_Returns_List()
        {
            //Arrange
            var list = new List<Promocion>
            {
                new Promocion()
            };
            Mock.Arrange(() => _promocionesServices.GetListAsync(_cancellationToken)).Returns(Task.FromResult(list));

            //Act
            var result = await _promocionesController.GetListAsync(_cancellationToken);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<List<Promocion>>());
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
