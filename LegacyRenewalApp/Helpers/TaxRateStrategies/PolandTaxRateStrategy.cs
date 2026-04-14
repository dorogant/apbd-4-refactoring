using LegacyRenewalApp.Interfaces;
namespace LegacyRenewalApp.Helpers.TaxRateStrategies;

public class PolandTaxRateStrategy : ITaxRateStrategy
{
    public decimal GetRate() => 0.23m;
}