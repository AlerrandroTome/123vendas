using _123vendas.Domain.Entities.Aggregates.Sales;

namespace _123vendas.Domain.Interfaces
{
    public interface ISaleItemRepository
    {
        Task AddRangeAsync(List<SaleItem> saleItems);
        Task UpdateRangeAsync(List<SaleItem> saleItems);
        Task DeleteRangeAsync(List<SaleItem> saleItems);
    }
}