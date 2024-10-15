using _123vendas.Domain.Entities.Aggregates.Sales;
using _123vendas.Domain.Interfaces;

namespace _123vendas.Data.Persistence.Repositories
{
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly SalesDbContext _context;

        public SaleItemRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(List<SaleItem> saleItems)
        {
            await _context.SaleItems.AddRangeAsync(saleItems);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<SaleItem> saleItems)
        {
            _context.SaleItems.RemoveRange(saleItems);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<SaleItem> saleItems)
        {
            _context.SaleItems.UpdateRange(saleItems);
            await _context.SaveChangesAsync();
        }
    }
}
