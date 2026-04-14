using LegacyRenewalApp.Interfaces;
namespace LegacyRenewalApp.Helpers.TaxRateStrategies;

public class DefaultTaxRateStrategy : ITaxRateStrategy
{
    public decimal GetRate() => 0.20m;
}