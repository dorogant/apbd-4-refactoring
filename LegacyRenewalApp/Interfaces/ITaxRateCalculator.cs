namespace LegacyRenewalApp.Interfaces;

public interface ITaxRateCalculator
{
    decimal Calculate(string country);
}