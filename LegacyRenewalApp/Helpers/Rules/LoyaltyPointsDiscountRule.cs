using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.Rules;

public class LoyaltyPointsDiscountRule : IDiscountRule
{
    public DiscountResult Apply(RenewalDiscountContext context)
    {
        var customer = context.Customer;

        if (!context.UseLoyaltyPoints || customer.LoyaltyPoints <= 0)
        {
            return DiscountResult.Empty();
        }

        int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;

        return new DiscountResult(pointsToUse, $"loyalty points used: {pointsToUse}; ");
    }
}