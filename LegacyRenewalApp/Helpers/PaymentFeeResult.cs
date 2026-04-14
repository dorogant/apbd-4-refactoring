namespace LegacyRenewalApp.Helpers;

public class PaymentFeeResult
{
    public decimal Amount { get; }
    public string Notes { get; }

    public PaymentFeeResult(decimal amount, string notes)
    {
        Amount = amount;
        Notes = notes;
    }
}