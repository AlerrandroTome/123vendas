using Microsoft.EntityFrameworkCore;
using _123vendas.Domain;
using _123vendas.Domain.Entities.Aggregates.Sales;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _123vendas.Data.Configurations
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(si => si.Id);

            builder.Property(si => si.ProductName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(si => si.UnitPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(si => si.Quantity)
                   .IsRequired();

            builder.Property(si => si.Discount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(si => si.TotalItemValue)
                   .HasColumnType("decimal(18,2)")
                   .HasComputedColumnSql("[UnitPrice] * [Quantity] - [Discount]");
        }
    }
}
