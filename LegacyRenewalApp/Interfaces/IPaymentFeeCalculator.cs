using LegacyRenewalApp.Helpers;

namespace LegacyRenewalApp.Interfaces;

public interface IPaymentFeeCalculator
{
    PaymentFeeResult Calculate(string paymentMethod, PaymentFeeContext context);
}