using LegacyRenewalApp.Interfaces;
namespace LegacyRenewalApp.Helpers.TaxRateStrategies;

public class CzechRepublicTaxRateStrategy : ITaxRateStrategy
{
    public decimal GetRate() => 0.21m;
}