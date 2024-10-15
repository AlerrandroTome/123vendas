namespace _123vendas.Domain.DTOs
{
    public class SaleDto
    {
        public SaleDto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public bool IsCanceled { get; set; } = false;
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
