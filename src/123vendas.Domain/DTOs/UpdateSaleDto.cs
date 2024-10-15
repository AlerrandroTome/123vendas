namespace _123vendas.Domain.DTOs
{
    public class UpdateSaleDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public List<UpdateSaleItemDto> Items { get; set; } = new();
    }
}
