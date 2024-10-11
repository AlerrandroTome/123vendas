using _123vendas.Domain.Entities.Aggregates.Sales;

namespace _123vendas.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale?> GetByIdAsync(Guid id);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Guid id);
    }
}
