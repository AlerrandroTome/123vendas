using _123vendas.Domain.DTOs;
using _123vendas.Domain.Entities.Aggregates.Sales;
using _123vendas.Domain.Interfaces;
using _123vendas.Domain.Mapping;
using _123vendas.Domain.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace _123vendas.Domain.Tests.Services
{
    public class SaleServiceTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleService> _logger;
        private readonly SaleService _saleService;

        public SaleServiceTests()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();

            _saleRepository = Substitute.For<ISaleRepository>();
            _saleItemRepository = Substitute.For<ISaleItemRepository>();
            _logger = Substitute.For<ILogger<SaleService>>();

            _saleService = new SaleService(_saleRepository, _saleItemRepository, _mapper, _logger);
        }

        [Fact]
        public async Task CreateSaleAsync_Should_Create_New_Sale()
        {
            // Arrange
            CreateSaleDto createSaleDto = new CreateSaleDto
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto>()
            };

            Sale expectedSale = _mapper.Map<Sale>(createSaleDto);

            // Act
            SaleDto result = await _saleService.CreateSaleAsync(createSaleDto);

            // Assert
            result.Should().NotBeNull();
            result.CustomerId.Should().Be(expectedSale.CustomerId);
            result.BranchId.Should().Be(expectedSale.BranchId);
            result.Items.Should().HaveCount(expectedSale.Items.Count);
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
        }

        [Fact]
        public async Task GetSaleByIdAsync_Should_Return_Sale_When_It_Exists()
        {
            // Arrange
            Sale sale = new Sale();
            _saleRepository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>()).Returns(sale);

            // Act
            SaleDto? result = await _saleService.GetSaleByIdAsync(sale.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
        }

        [Fact]
        public async Task GetSaleByIdAsync_Should_Return_Null_If_Sale_Does_Not_Exist()
        {
            // Arrange
            _saleRepository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>()).Returns((Sale)null);

            // Act
            SaleDto? result = await _saleService.GetSaleByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllSalesAsync_Should_Return_List_Of_Sales()
        {
            // Arrange
            List<Sale> sales = new List<Sale> { new Sale(), new Sale() };
            _saleRepository.GetAllAsync().Returns(sales);

            // Act
            IEnumerable<SaleDto> result = await _saleService.GetAllSalesAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateSaleAsync_Should_Update_Sale_And_Items()
        {
            // Arrange
            Guid idProduct1 = new Guid("1494479b-b0ae-4c6e-8fd5-654a107eb782");
            Guid idProduct2 = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa1");
            Sale sale = new Sale();
            UpdateSaleDto updateSaleDto = new UpdateSaleDto
            {
                Id = sale.Id,
                Items = new List<UpdateSaleItemDto>
                {
                    new UpdateSaleItemDto
                    {
                        ProductId = idProduct1,
                        ProductName = "Produto de teste 1.1",
                        UnitPrice = 15,
                        Quantity = 7,
                        Discount = 0
                    },
                    new UpdateSaleItemDto
                    {
                        ProductId = idProduct2,
                        ProductName = "Produto de teste 1.2",
                        UnitPrice = 45,
                        Quantity = 1,
                        Discount = 5
                    }
                }
            };

            Sale expectedResult = _mapper.Map<Sale>(updateSaleDto);

            _saleRepository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>()).Returns(sale);

            // Act
            SaleDto? result = await _saleService.UpdateSaleAsync(updateSaleDto);

            // Assert
            result.Should().NotBeNull();
            result.Items[0].Quantity.Should().Be(expectedResult.Items.FirstOrDefault(i => i.ProductId.Equals(idProduct1)).Quantity);
            result.Items[1].Quantity.Should().Be(expectedResult.Items.FirstOrDefault(i => i.ProductId.Equals(idProduct2)).Quantity);
            await _saleItemRepository.Received(1).AddRangeAsync(Arg.Is<List<SaleItem>>(x =>
                x.Count == 2));
        }

        [Fact]
        public async Task UpdateSaleAsync_Should_Return_Null_If_Sale_Does_Not_Exist()
        {
            // Arrange
            UpdateSaleDto updateSaleDto = new UpdateSaleDto { Id = Guid.NewGuid() };
            _saleRepository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>()).Returns((Sale)null);

            // Act
            SaleDto? result = await _saleService.UpdateSaleAsync(updateSaleDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CancelSaleAsync_Should_Cancel_Sale_When_It_Exists()
        {
            // Arrange
            Sale sale = new Sale();
            _saleRepository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>()).Returns(sale);

            // Act
            bool result = await _saleService.CancelSaleAsync(sale.Id);

            // Assert
            result.Should().BeTrue();
            await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>());
        }

        [Fact]
        public async Task CancelSaleAsync_Should_Return_False_If_Sale_Already_Canceled()
        {
            // Arrange
            Sale sale = new Sale();
            sale.CancelSale();
            _saleRepository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>()).Returns(sale);

            // Act
            bool result = await _saleService.CancelSaleAsync(sale.Id);

            // Assert
            result.Should().BeFalse();
            await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
        }

        [Fact]
        public async Task CancelSaleAsync_Should_Return_False_If_Sale_Not_Found()
        {
            // Arrange
            _saleRepository.GetByIdWithNoTrackingAsync(Arg.Any<Guid>()).Returns((Sale)null);

            // Act
            bool result = await _saleService.CancelSaleAsync(Guid.NewGuid());

            // Assert
            result.Should().BeFalse();
        }
    }
}