using System.Collections.Generic;
using LegacyRenewalApp.Helpers;
using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Models;

public class PaymentFeeCalculator : IPaymentFeeCalculator
{
    public Dictionary<string, IPaymentFeeStrategy> PaymentFeeStrategies { get; }

    public PaymentFeeCalculator(Dictionary<string, IPaymentFeeStrategy> paymentFeeStrategies)
    {
        PaymentFeeStrategies = paymentFeeStrategies;
    }

    public PaymentFeeResult Calculate(string paymentMethod, PaymentFeeContext context)
    {
        if (PaymentFeeStrategies.TryGetValue(paymentMethod, out IPaymentFeeStrategy strategy))
        {
            return strategy.Calculate(context);
        };
        
        throw new KeyNotFoundException($"Strategy {paymentMethod} does not exist");
    }
}