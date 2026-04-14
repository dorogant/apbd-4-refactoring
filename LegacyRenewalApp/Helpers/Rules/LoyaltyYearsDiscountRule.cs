using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.Rules;

public class LoyaltyYearsDiscountRule : IDiscountRule
{
    public DiscountResult Apply(RenewalDiscountContext context)
    {
        int years = context.Customer.YearsWithCompany;
        decimal baseAmount = context.BaseAmount;

        if (years >= 5)
        {
            return new DiscountResult(baseAmount * 0.07m, "long-term loyalty discount; ");
        }

        if (years >= 2)
        {
            return new DiscountResult(baseAmount * 0.03m, "basic loyalty discount; ");
        }

        return DiscountResult.Empty();
    }
}