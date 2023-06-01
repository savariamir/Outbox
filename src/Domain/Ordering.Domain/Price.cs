using Anshan.Domain;
using Anshan.Domain.Exceptions;

namespace Ordering.Domain;

public class Price : ValueObject
{
    public static readonly Price None = new Price(0, 0, 0, 0);

    public static readonly decimal Zero = 0M;
    public decimal DueBalance { get; private set; }

    public decimal Amount { get; private set; }

    public decimal ShippingAmount { get; private set; }

    public decimal DiscountAmount { get; private set; }

    public Price(decimal amount, decimal shippingAmount, decimal discountAmount, decimal dueBalance)
    {
        DueBalance = dueBalance;
        ShippingAmount = shippingAmount;
        DiscountAmount = discountAmount;
        Amount = amount;
    }

    public static Price operator +(Price price1, Price price2)
    {
        var totalAmount = price1.DueBalance + price2.DueBalance;
        var shippingAmount = price1.ShippingAmount + price2.ShippingAmount;
        var discountAmount = price1.DiscountAmount + price2.DiscountAmount;
        var amount = price1.Amount + price2.Amount;

        EnsureAmountsAreNotNegative(totalAmount, shippingAmount, discountAmount, amount);

        return new Price(amount, shippingAmount, discountAmount, totalAmount);
    }

    private static void EnsureAmountsAreNotNegative(decimal totalAmount, decimal shippingAmount, decimal discountAmount, decimal amount)
    {
        if (totalAmount < 0 || shippingAmount < 0 || discountAmount < 0 || amount < 0)
            throw new DomainException("1", "Amounts should not be negative");
    }

    public static Price operator -(Price price1, Price price2)
    {
        var totalAmount = price1.DueBalance - price2.DueBalance;
        var shippingAmount = price1.ShippingAmount - price2.ShippingAmount;
        var discountAmount = price1.DiscountAmount - price2.DiscountAmount;
        var amount = price1.Amount - price2.Amount;

        EnsureAmountsAreNotNegative(totalAmount, shippingAmount, discountAmount, amount);

        return new Price(amount, shippingAmount, discountAmount, totalAmount);
    }

    public static Price GetPrice(Order order)
    {
        var amount = order.Items.Aggregate(Zero, (sum, next) => sum + (next.UnitPrice * next.Quantity));
        var discountAmount = order.Items.Aggregate(Zero, (sum, next) => sum + (next.DiscountAmount));
        var total = amount + order.Shipping.Cost - discountAmount;

        return new Price(amount, order.Shipping.Cost, discountAmount, total);
    }
}