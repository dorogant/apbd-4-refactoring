using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.SupportFeeStrategies;

public class StartSupportFeeStrategy : ISupportFeeStrategy
{
    public decimal GetFee() => 250m;
}