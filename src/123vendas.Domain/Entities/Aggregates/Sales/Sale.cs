using _123vendas.Domain.DTOs;
using _123vendas.Domain.Enums;

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
        
        public Sale()
        {
            Id = Guid.NewGuid();
            Items = new List<SaleItem>();
        }

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

        public void UpdateSale(UpdateSaleDto saleDto)
        {
            BranchName = saleDto.BranchName;
            BranchId = saleDto.BranchId;
            CustomerName = saleDto.CustomerName;
            CustomerId = saleDto.CustomerId;
        }

        public Dictionary<string, List<SaleItem>> UpdateSaleItems(UpdateSaleDto saleDto)
        {
            List<SaleItem> existingItems = Items.ToList();
            Dictionary<string, List<SaleItem>> actionsToTake = new Dictionary<string, List<SaleItem>>
            {
                { ESaleActions.ToBeDeleted.ToString(), new List<SaleItem>() },
                { ESaleActions.ToBeAdded.ToString(), new List<SaleItem>() },
                { ESaleActions.ToBeUpdated.ToString(), new List<SaleItem>() }
            };

            foreach (SaleItem? existingItem in existingItems)
            {
                UpdateSaleItemDto? newItem = saleDto.Items.FirstOrDefault(i => i.Id == existingItem.Id);
                if (newItem == null)
                {
                    actionsToTake[ESaleActions.ToBeDeleted.ToString()].Add(existingItem);
                    Items.Remove(existingItem);
                }
                else
                {
                    existingItem.UpdateSaleItem(newItem.UnitPrice, newItem.Quantity, newItem.Discount);
                    actionsToTake[ESaleActions.ToBeUpdated.ToString()].Add(existingItem);
                }
            }

            IEnumerable<UpdateSaleItemDto> newItems = saleDto.Items.Where(s => !s.Id.HasValue);
            foreach (UpdateSaleItemDto newItem in newItems)
            {
                actionsToTake[ESaleActions.ToBeAdded.ToString()].Add(AddItem(newItem.ProductId, newItem.ProductName, newItem.UnitPrice, newItem.Quantity, newItem.Discount));
            }

            return actionsToTake;
        }

        private SaleItem AddItem(Guid productId, string productName, decimal unitPrice, int quantity, decimal discount)
        {
            SaleItem newItem = new SaleItem(productId, productName, unitPrice, quantity, discount);
            Items.Add(newItem);
            return newItem;
        }

        public void CalculateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (SaleItem item in Items)
            {
                totalAmount += item.TotalItemValue;
            }

            TotalAmount = totalAmount;
        }

        public void CancelSale()
        {
            IsCanceled = true;
        }
    }
}
