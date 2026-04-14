namespace LegacyRenewalApp.Interfaces;

public interface ISupportFeeCalculator
{
    decimal Calculate(string planCode);
}