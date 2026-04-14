using System.Collections.Generic;
using LegacyRenewalApp.Helpers;
using LegacyRenewalApp.Interfaces;

namespace LegacyRenewalApp.Models
{
    public class DiscountCalculator : IDiscountCalculator
    {
        private readonly IEnumerable<IDiscountRule> _rules;

        public DiscountCalculator(IEnumerable<IDiscountRule> rules)
        {
            _rules = rules;
        }

        public DiscountResult Calculate(RenewalDiscountContext context)
        {
            decimal totalAmount = 0m;
            string notes = string.Empty;

            foreach (var rule in _rules)
            {
                var result = rule.Apply(context);
                totalAmount += result.Amount;
                notes += result.Notes;
            }

            return new DiscountResult(totalAmount, notes);
        }
    }
}