using LegacyRenewalApp.Interfaces;
namespace LegacyRenewalApp.Helpers.TaxRateStrategies;

public class GermanyTaxRateStrategy : ITaxRateStrategy
{
    public decimal GetRate() => 0.19m;
}