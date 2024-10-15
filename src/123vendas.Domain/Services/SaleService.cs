using _123vendas.Domain.DTOs;
using _123vendas.Domain.Entities.Aggregates.Sales;
using _123vendas.Domain.Enums;
using _123vendas.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace _123vendas.Domain.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleService> _logger;

        public SaleService(ISaleRepository saleRepository, ISaleItemRepository saleItemRepository, IMapper mapper, ILogger<SaleService> logger)
        {
            _saleRepository = saleRepository;
            _saleItemRepository = saleItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto)
        {
            Sale sale = _mapper.Map<Sale>(createSaleDto);

            await _saleRepository.AddAsync(sale);
            LogEvent("SaleCreated", sale.Id);
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<SaleDto?> GetSaleByIdAsync(Guid id)
        {
            Sale? sale = await _saleRepository.GetByIdWithNoTrackingAsync(id);
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<IEnumerable<SaleDto>> GetAllSalesAsync()
        {
            var sales = await _saleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<SaleDto?> UpdateSaleAsync(UpdateSaleDto updateSaleDto)
        {
            Sale? sale = await _saleRepository.GetByIdWithNoTrackingAsync(updateSaleDto.Id);
            if (sale == null) return null;

            sale.UpdateSale(updateSaleDto);
            Dictionary<string, List<SaleItem>> actionsToTakeIntoItems = sale.UpdateSaleItems(updateSaleDto);

            foreach (string action in actionsToTakeIntoItems.Keys)
            {
                if(actionsToTakeIntoItems[action].Any())
                {
                    switch(Enum.Parse(typeof(ESaleActions), action))
                    {
                        case ESaleActions.ToBeAdded:
                            await _saleItemRepository.AddRangeAsync(actionsToTakeIntoItems[action]);
                            break;

                        case ESaleActions.ToBeUpdated:
                            await _saleItemRepository.UpdateRangeAsync(actionsToTakeIntoItems[action]);
                            break;

                        case ESaleActions.ToBeDeleted:
                            await _saleItemRepository.DeleteRangeAsync(actionsToTakeIntoItems[action]);
                            break;
                    }
                }
            }

            sale.CalculateTotalAmount();
            await _saleRepository.UpdateAsync(sale);
            LogEvent("SaleUpdated", updateSaleDto.Id);
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<bool> CancelSaleAsync(Guid id)
        {
            Sale? existingSale = await _saleRepository.GetByIdWithNoTrackingAsync(id);
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