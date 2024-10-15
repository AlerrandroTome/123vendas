using _123vendas.Domain.DTOs;

namespace _123vendas.Domain.Services
{
    public interface ISaleService
    {
        Task<SaleDto> CreateSaleAsync(CreateSaleDto saleDto);
        Task<SaleDto?> GetSaleByIdAsync(Guid id);
        Task<IEnumerable<SaleDto>> GetAllSalesAsync();
        Task<SaleDto?> UpdateSaleAsync(UpdateSaleDto saleDto);
        Task<bool> CancelSaleAsync(Guid id);
    }
}