namespace LegacyRenewalApp.Helpers;

public class DiscountResult
{
    public decimal Amount { get; }
    public string Notes { get; }

    public DiscountResult(decimal amount, string notes)
    {
        Amount = amount;
        Notes = notes;
    }

    public static DiscountResult Empty()
    {
        return new DiscountResult(0m, string.Empty);
    }
}