namespace _123vendas.Domain.DTOs
{
    public class SaleItemDto
    {
        public SaleItemDto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemValue { get; set; }
    }
}
