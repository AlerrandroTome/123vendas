using _123vendas.Domain.Entities.Aggregates.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _123vendas.Data.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SaleDate)
                   .IsRequired();

            builder.Property(s => s.TotalAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(s => s.IsCanceled)
                   .IsRequired();

            builder.HasMany(s => s.Items)
                   .WithOne()
                   .HasForeignKey("SaleId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(s => s.CustomerName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.BranchName)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
