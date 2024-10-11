using _123vendas.Domain.DTOs;

namespace _123vendas.Domain.Services
{
    public interface ISaleService
    {
        Task<SaleDto> CreateSaleAsync(SaleDto saleDto);
        Task<SaleDto?> GetSaleByIdAsync(Guid id);
        Task<IEnumerable<SaleDto>> GetAllSalesAsync();
        Task<SaleDto?> UpdateSaleAsync(SaleDto saleDto);
        Task<bool> CancelSaleAsync(Guid id);
    }
}