﻿namespace _123vendas.Domain.Entities.Aggregates.Sales
{
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalItemValue { get; private set; }

        public SaleItem(Guid productId, string productName, decimal unitPrice, int quantity, decimal discount)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
            CalculateTotalItemValue();
        }

        internal void UpdateSaleItem(decimal unitPrice, int quantity, decimal discount)
        {
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;

            CalculateTotalItemValue();
        }

        internal void CalculateTotalItemValue()
        {
            decimal total = UnitPrice * Quantity - Discount;
            TotalItemValue = total < 0 ? 0 : total;
        }
    }
}
