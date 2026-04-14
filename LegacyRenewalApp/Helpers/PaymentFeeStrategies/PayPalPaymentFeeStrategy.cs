using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.PaymentFeeStrategies;

public class PayPalPaymentFeeStrategy : IPaymentFeeStrategy
{
    public PaymentFeeResult Calculate(PaymentFeeContext context)
    {
        return new PaymentFeeResult((context.SubTotalAfterDiscount + context.SupportFee) * 0.035m, "paypal fee; ");
    }
}