using _123vendas.Domain.DTOs;
using _123vendas.Domain.Entities.Aggregates.Sales;
using _123vendas.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace _123vendas.Domain.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleService> _logger;

        public SaleService(ISaleRepository saleRepository, IMapper mapper, ILogger<SaleService> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SaleDto> CreateSaleAsync(SaleDto saleDto)
        {
            Sale sale = _mapper.Map<Sale>(saleDto);
            await _saleRepository.AddAsync(sale);
            LogEvent("SaleCreated", sale.Id);
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<SaleDto?> GetSaleByIdAsync(Guid id)
        {
            Sale? sale = await _saleRepository.GetByIdAsync(id);
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<IEnumerable<SaleDto>> GetAllSalesAsync()
        {
            var sales = await _saleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<SaleDto?> UpdateSaleAsync(SaleDto saleDto)
        {
            Sale? sale = await _saleRepository.GetByIdAsync(saleDto.Id);
            if (sale == null) return null;

            List<SaleItem> existingItems = sale.Items.ToList();

            foreach (SaleItem? existingItem in existingItems)
            {
                SaleItemDto? newItem = saleDto.Items.FirstOrDefault(i => i.ProductId == existingItem.ProductId);
                if (newItem == null)
                {
                    sale.RemoveItem(existingItem.ProductId);
                }
                else
                {
                    sale.UpdateItem(existingItem.ProductId, newItem.Quantity, newItem.Discount);
                }
            }

            foreach (SaleItemDto newItem in saleDto.Items)
            {
                if (!existingItems.Any(i => i.ProductId == newItem.ProductId))
                {
                    sale.AddItem(newItem.ProductId, newItem.ProductName, newItem.UnitPrice, newItem.Quantity, newItem.Discount);
                }
            }

            await _saleRepository.UpdateAsync(sale);

            LogEvent("SaleUpdated", saleDto.Id);

            return saleDto;
        }

        public async Task<bool> CancelSaleAsync(Guid id)
        {
            Sale? existingSale = await _saleRepository.GetByIdAsync(id);
            if (existingSale == null || existingSale.IsCanceled) return false;

            existingSale.CancelSale();
            await _saleRepository.UpdateAsync(existingSale);
            _logger.LogInformation($"Sale canceled with ID: {existingSale.Id}");

            LogEvent("SaleCancelled", existingSale.Id);

            return true;
        }

        private void LogEvent(string eventName, Guid saleId)
        {
            _logger.LogInformation($"Event: {eventName}, Sale ID: {saleId}");
        }
    }
}
