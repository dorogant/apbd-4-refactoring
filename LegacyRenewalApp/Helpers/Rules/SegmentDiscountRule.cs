
using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.Rules;

public class SegmentDiscountRule : IDiscountRule
{
    public DiscountResult Apply(RenewalDiscountContext context)
    {
        var customer = context.Customer;
        var plan = context.Plan;
        decimal baseAmount = context.BaseAmount;

        if (customer.Segment == "Silver")
        {
            return new DiscountResult(baseAmount * 0.05m, "silver discount; ");
        }

        if (customer.Segment == "Gold")
        {
            return new DiscountResult(baseAmount * 0.10m, "gold discount; ");
        }

        if (customer.Segment == "Platinum")
        {
            return new DiscountResult(baseAmount * 0.15m, "platinum discount; ");
        }

        if (customer.Segment == "Education" && plan.IsEducationEligible)
        {
            return new DiscountResult(baseAmount * 0.20m, "education discount; ");
        }

        return DiscountResult.Empty();
    }
}