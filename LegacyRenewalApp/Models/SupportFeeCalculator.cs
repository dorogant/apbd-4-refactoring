using System.Collections.Generic;
using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Models;

public class SupportFeeCalculator : ISupportFeeCalculator
{
    private readonly Dictionary<string, ISupportFeeStrategy> _strategies;
    public SupportFeeCalculator(Dictionary<string, ISupportFeeStrategy> strategies)
        => _strategies = strategies;

    public decimal Calculate(string planCode)
        => _strategies.TryGetValue(planCode, out var s) ? s.GetFee() : 0m;
}