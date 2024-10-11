namespace _123vendas.Domain.Entities.External
{
    public class Product
    {
        public Product(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
    }
}
