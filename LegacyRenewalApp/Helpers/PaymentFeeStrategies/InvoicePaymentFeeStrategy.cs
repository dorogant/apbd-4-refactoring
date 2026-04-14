using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.PaymentFeeStrategies;

public class InvoicePaymentFeeStrategy : IPaymentFeeStrategy
{
    public PaymentFeeResult Calculate(PaymentFeeContext context)
    {
        return new PaymentFeeResult(0m, "invoice payment; ");
    }
}