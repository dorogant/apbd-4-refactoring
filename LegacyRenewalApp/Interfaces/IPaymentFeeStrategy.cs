using LegacyRenewalApp.Helpers;

namespace LegacyRenewalApp.Interfaces;

public interface IPaymentFeeStrategy
{ 
    PaymentFeeResult Calculate(PaymentFeeContext context);
}