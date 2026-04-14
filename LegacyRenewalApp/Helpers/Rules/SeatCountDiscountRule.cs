using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.Rules;

public class SeatCountDiscountRule : IDiscountRule
{
    public DiscountResult Apply(RenewalDiscountContext context)
    {
        int seatCount = context.SeatCount;
        decimal baseAmount = context.BaseAmount;

        if (seatCount >= 50)
        {
            return new DiscountResult(baseAmount * 0.12m, "large team discount; ");
        }

        if (seatCount >= 20)
        {
            return new DiscountResult(baseAmount * 0.08m, "medium team discount; ");
        }

        if (seatCount >= 10)
        {
            return new DiscountResult(baseAmount * 0.04m, "small team discount; ");
        }

        return DiscountResult.Empty();
    }
}