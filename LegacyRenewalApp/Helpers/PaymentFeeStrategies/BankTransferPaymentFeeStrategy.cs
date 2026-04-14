using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Helpers.PaymentFeeStrategies;

public class BankTransferPaymentFeeStrategy : IPaymentFeeStrategy
{
    public PaymentFeeResult Calculate(PaymentFeeContext context)
    {
        return new PaymentFeeResult((context.SubTotalAfterDiscount + context.SupportFee) * 0.01m, "bank transfer fee; ");
    }
}