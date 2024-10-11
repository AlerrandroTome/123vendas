using _123vendas.Domain.Entities.Aggregates.Sales;
using _123vendas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _123vendas.Data.Persistence.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SalesDbContext _context;

        public SaleRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            return await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales.Include(s => s.Items).ToListAsync();
        }

        public async Task AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Sale? sale = await GetByIdAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }
    }
}