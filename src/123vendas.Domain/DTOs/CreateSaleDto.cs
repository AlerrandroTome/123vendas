namespace _123vendas.Domain.DTOs
{
    public class CreateSaleDto
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public List<CreateSaleItemDto> Items { get; set; } = new();
    }
}
