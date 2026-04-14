using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.SupportFeeStrategies;

public class ProSupportFeeStrategy : ISupportFeeStrategy
{
    public decimal GetFee() => 400m;
}