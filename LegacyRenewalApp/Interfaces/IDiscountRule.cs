using LegacyRenewalApp.Helpers;

namespace LegacyRenewalApp.Interfaces
{
    public interface IDiscountRule
    {
        DiscountResult Apply(RenewalDiscountContext context);
    }
}