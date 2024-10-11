namespace _123vendas.Domain.DTOs
{
    public class SaleDto
    {
        public SaleDto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public decimal TotalAmount { get; private set; } = 0;
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; }
        public bool IsCanceled { get; private set; } = false;
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
