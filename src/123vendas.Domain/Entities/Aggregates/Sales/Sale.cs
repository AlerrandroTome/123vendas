namespace _123vendas.Domain.Entities.Aggregates.Sales
{
    public class Sale
    {
        public Guid Id { get; private set; }
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public decimal TotalAmount { get; private set; }
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; }
        public bool IsCanceled { get; private set; }
        public ICollection<SaleItem> Items { get; private set; }

        public Sale(Guid customerId, string customerName, Guid branchId, string branchName)
        {
            Id = Guid.NewGuid();
            SaleDate = DateTime.UtcNow;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            Items = new List<SaleItem>();
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity, decimal discount)
        {
            var saleItem = new SaleItem(productId, productName, unitPrice, quantity, discount);
            Items.Add(saleItem);
            CalculateTotalAmount();
        }

        public void RemoveItem(Guid productId)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);
                CalculateTotalAmount();
            }
        }

        public void UpdateItem(Guid productId, int quantity, decimal discount)
        {
            SaleItem? existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(quantity);
                existingItem.UpdateDiscount(discount);
                CalculateTotalAmount();
            }
        }

        public void CancelSale()
        {
            IsCanceled = true;
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = 0;
            foreach (SaleItem item in Items)
            {
                TotalAmount += item.TotalItemValue;
            }
        }
    }
}
