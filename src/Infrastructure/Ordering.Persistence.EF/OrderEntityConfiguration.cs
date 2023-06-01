using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain;

namespace Ordering.Persistence.EF;

public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(p => p.Id);

        builder.OwnsOne(o => o.Shipping, oi =>
        {
            oi.Property(p => p.Cost);
            oi.Property(p => p.DeliveryDate);
            oi.Property(p => p.EarliestDeliveryTime);
            oi.Property(p => p.LatestDeliveryTime);
        });
        
        builder.OwnsOne(o => o.Address, oi =>
        {
            oi.Property(p => p.RecipientFullName);
            oi.Property(p => p.RecipientPhoneNumber);
            oi.Property(p => p.Line);
            oi.Property(p => p.PostalCode);
        });

        builder.OwnsOne(o => o.Price, oi =>
        {
            oi.Property(p => p.DueBalance);
            oi.Property(p => p.ShippingAmount);
            oi.Property(p => p.Amount);
            oi.Property(p => p.DiscountAmount);
        });

        builder.OwnsMany(o => o.Items, oi =>
        {
            oi.WithOwner().HasForeignKey("OrderId");
            oi.Property<int>("Id");
            oi.Property(cm => cm.ProductId);
            oi.Property(cm => cm.Discount);
            oi.Property(cm => cm.DiscountAmount);
            oi.Property(cm => cm.ProductName);
            oi.Property(cm => cm.Quantity);
            oi.Property(cm => cm.UnitPrice);
            oi.HasKey("Id");
        });
    }
}