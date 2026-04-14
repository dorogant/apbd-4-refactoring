using LegacyRenewalApp.Helpers;

namespace LegacyRenewalApp.Interfaces;

public interface IDiscountCalculator
{
    DiscountResult Calculate(RenewalDiscountContext context);
}