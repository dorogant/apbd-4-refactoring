using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.SupportFeeStrategies;

public class EnterpriseSupportFeeStrategy : ISupportFeeStrategy
{
    public decimal GetFee() => 700m;
}