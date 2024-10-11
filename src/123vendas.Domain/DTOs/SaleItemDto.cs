namespace _123vendas.Domain.DTOs
{
    public class SaleItemDto
    {
        public SaleItemDto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalItemValue { get; private set; }
    }
}
