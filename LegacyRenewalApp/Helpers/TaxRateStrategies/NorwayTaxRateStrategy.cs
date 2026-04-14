using LegacyRenewalApp.Interfaces;
namespace LegacyRenewalApp.Helpers.TaxRateStrategies;

public class NorwayTaxRateStrategy : ITaxRateStrategy
{
    public decimal GetRate() => 0.25m;
}