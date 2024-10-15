namespace _123vendas.Domain.Entities.External
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public Customer(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
