namespace LegacyRenewalApp.Helpers;

public class RenewalDiscountContext
{
    public Customer Customer { get; }
    public SubscriptionPlan Plan { get; }
    public int SeatCount { get; }
    public bool UseLoyaltyPoints { get; }
    public decimal BaseAmount { get; }

    public RenewalDiscountContext(
        Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        bool useLoyaltyPoints,
        decimal baseAmount)
    {
        Customer = customer;
        Plan = plan;
        SeatCount = seatCount;
        UseLoyaltyPoints = useLoyaltyPoints;
        BaseAmount = baseAmount;
    }
}