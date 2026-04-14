using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.PaymentFeeStrategies;

public class CardPaymentFeeStrategy : IPaymentFeeStrategy
{
    public PaymentFeeResult Calculate(PaymentFeeContext context)
    {
        return new PaymentFeeResult((context.SubTotalAfterDiscount + context.SupportFee) * 0.02m, "card payment fee; ");
    }
}