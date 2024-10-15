using _123vendas.api.Controllers;
using _123vendas.Domain.DTOs;
using _123vendas.Domain.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace _123vendas.api.Tests.Controllers
{
    public class SaleControllerTests
    {
        private readonly ISaleService _saleService;
        private readonly SaleController _saleController;

        public SaleControllerTests()
        {
            _saleService = Substitute.For<ISaleService>();
            _saleController = new SaleController(_saleService);
        }

        [Fact]
        public async Task GetSaleByIdAsync_ShouldReturn_NotFound_WhenSaleDoesNotExist()
        {
            // Arrange
            _saleService.GetSaleByIdAsync(Arg.Any<Guid>()).Returns((SaleDto)null);

            // Act
            IActionResult result = await _saleController.GetSaleByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetSaleByIdAsync_ShouldReturn_Ok_WhenSaleExists()
        {
            // Arrange
            SaleDto saleDto = new SaleDto { Id = Guid.NewGuid() };
            _saleService.GetSaleByIdAsync(Arg.Any<Guid>()).Returns(saleDto);

            // Act
            IActionResult result = await _saleController.GetSaleByIdAsync(saleDto.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(saleDto);
        }

        [Fact]
        public async Task GetAllSalesAsync_ShouldReturn_Ok_With_List_Of_Sales()
        {
            // Arrange
            List<SaleDto> sales = new List<SaleDto> { new SaleDto { Id = Guid.NewGuid() }, new SaleDto { Id = Guid.NewGuid() } };
            _saleService.GetAllSalesAsync().Returns(sales);

            // Act
            IActionResult result = await _saleController.GetAllSalesAsync();

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(sales);
        }

        [Fact]
        public async Task CreateSaleAsync_Should_Return_CreatedAtAction()
        {
            // Arrange
            CreateSaleDto createSaleDto = new CreateSaleDto();
            SaleDto saleDto = new SaleDto { Id = Guid.NewGuid() };

            _saleService.CreateSaleAsync(Arg.Any<CreateSaleDto>()).Returns(saleDto);

            // Act
            IActionResult result = await _saleController.CreateSaleAsync(createSaleDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task UpdateSaleAsync_Should_Return_NoContent_When_Sale_Is_Updated()
        {
            // Arrange
            UpdateSaleDto updateSaleDto = new UpdateSaleDto { Id = Guid.NewGuid() };
            _saleService.UpdateSaleAsync(Arg.Any<UpdateSaleDto>()).Returns(new SaleDto());

            // Act
            IActionResult result = await _saleController.UpdateSaleAsync(updateSaleDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateSaleAsync_Should_Return_NotFound_If_Sale_Does_Not_Exist()
        {
            // Arrange
            UpdateSaleDto updateSaleDto = new UpdateSaleDto { Id = Guid.NewGuid() };
            _saleService.UpdateSaleAsync(Arg.Any<UpdateSaleDto>()).Returns((SaleDto)null);

            // Act
            IActionResult result = await _saleController.UpdateSaleAsync(updateSaleDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CancelSaleAsync_Should_Return_NoContent_When_Sale_Is_Cancelled()
        {
            // Arrange
            _saleService.CancelSaleAsync(Arg.Any<Guid>()).Returns(true);

            // Act
            IActionResult result = await _saleController.CancelSaleAsync(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task CancelSaleAsync_Should_Return_NotFound_If_Sale_Not_Found_Or_Already_Cancelled()
        {
            // Arrange
            _saleService.CancelSaleAsync(Arg.Any<Guid>()).Returns(false);

            // Act
            IActionResult result = await _saleController.CancelSaleAsync(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
