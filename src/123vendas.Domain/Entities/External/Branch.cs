namespace _123vendas.Domain.Entities.External
{
    public class Branch
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public Branch(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
