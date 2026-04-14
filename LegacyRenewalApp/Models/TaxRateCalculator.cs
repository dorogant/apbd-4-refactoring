using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;
namespace LegacyRenewalApp.Models;

public class TaxRateCalculator : ITaxRateCalculator
{
    private readonly Dictionary<string, ITaxRateStrategy> _strategies;
    private readonly ITaxRateStrategy _default;

    public TaxRateCalculator(Dictionary<string, ITaxRateStrategy> strategies, ITaxRateStrategy defaultStrategy)
    {
        _strategies = strategies;
        _default = defaultStrategy;
    }

    public decimal Calculate(string country)
        => _strategies.TryGetValue(country, out var s) ? s.GetRate() : _default.GetRate();
}