using System;
using System.Collections.Generic;
using LegacyRenewalApp.Helpers;
using LegacyRenewalApp.Helpers.PaymentFeeStrategies;
using LegacyRenewalApp.Helpers.Rules;
using LegacyRenewalApp.Helpers.SupportFeeStrategies;
using LegacyRenewalApp.Helpers.TaxRateStrategies;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IRenewalServiceValidator _validator;
        private readonly IDiscountCalculator _discountCalculator;
        private readonly IPaymentFeeCalculator _paymentFeeCalculator;
        private readonly ISupportFeeCalculator _supportFeeCalculator;
        private readonly ITaxRateCalculator _taxRateCalculator;

        public SubscriptionRenewalService()
            : this(
                new CustomerRepository(),
                new SubscriptionPlanRepository(),
                new RenewalServiceValidator(),
                new DiscountCalculator(new List<IDiscountRule>
                {
                    new SegmentDiscountRule(),
                    new LoyaltyYearsDiscountRule(),
                    new SeatCountDiscountRule(),
                    new LoyaltyPointsDiscountRule()
                }),
                new PaymentFeeCalculator(new Dictionary<string, IPaymentFeeStrategy>
                {
                    { "CARD", new CardPaymentFeeStrategy() },
                    { "BANK_TRANSFER", new BankTransferPaymentFeeStrategy() },
                    { "PAYPAL", new PayPalPaymentFeeStrategy() },
                    { "INVOICE", new InvoicePaymentFeeStrategy() }
                }),
                new SupportFeeCalculator(new Dictionary<string, ISupportFeeStrategy>
                {
                    { "START", new StartSupportFeeStrategy() },
                    { "PRO", new ProSupportFeeStrategy() },
                    { "ENTERPRISE", new EnterpriseSupportFeeStrategy() }
                }),
                new TaxRateCalculator(new Dictionary<string, ITaxRateStrategy>
                {
                    { "Poland", new PolandTaxRateStrategy() },
                    { "Germany", new GermanyTaxRateStrategy() },
                    { "Czech Republic", new CzechRepublicTaxRateStrategy() },
                    { "Norway", new NorwayTaxRateStrategy() }
                }, new DefaultTaxRateStrategy()))
        {
        }

        public SubscriptionRenewalService(
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository subscriptionPlanRepository,
            IRenewalServiceValidator validator)
            : this(
                customerRepository,
                subscriptionPlanRepository,
                validator,
                new DiscountCalculator(new List<IDiscountRule>
                {
                    new SegmentDiscountRule(),
                    new LoyaltyYearsDiscountRule(),
                    new SeatCountDiscountRule(),
                    new LoyaltyPointsDiscountRule()
                }),
                new PaymentFeeCalculator(new Dictionary<string, IPaymentFeeStrategy>
                {
                    { "CARD", new CardPaymentFeeStrategy() },
                    { "BANK_TRANSFER", new BankTransferPaymentFeeStrategy() },
                    { "PAYPAL", new PayPalPaymentFeeStrategy() },
                    { "INVOICE", new InvoicePaymentFeeStrategy() }
                }),
                new SupportFeeCalculator(new Dictionary<string, ISupportFeeStrategy>
                {
                    { "START", new StartSupportFeeStrategy() },
                    { "PRO", new ProSupportFeeStrategy() },
                    { "ENTERPRISE", new EnterpriseSupportFeeStrategy() }
                }),
                new TaxRateCalculator(new Dictionary<string, ITaxRateStrategy>
                {
                    { "Poland", new PolandTaxRateStrategy() },
                    { "Germany", new GermanyTaxRateStrategy() },
                    { "Czech Republic", new CzechRepublicTaxRateStrategy() },
                    { "Norway", new NorwayTaxRateStrategy() }
                }, new DefaultTaxRateStrategy()))
        {
        }

        public SubscriptionRenewalService(
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository subscriptionPlanRepository,
            IRenewalServiceValidator validator,
            IDiscountCalculator discountCalculator,
            IPaymentFeeCalculator paymentFeeCalculator,
            ISupportFeeCalculator supportFeeCalculator,
            ITaxRateCalculator taxRateCalculator)
        {
            _customerRepository = customerRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _validator = validator;
            _discountCalculator = discountCalculator;
            _paymentFeeCalculator = paymentFeeCalculator;
            _supportFeeCalculator = supportFeeCalculator;
            _taxRateCalculator = taxRateCalculator;
        }

        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            _validator.Validate(customerId, planCode, seatCount, paymentMethod);

            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customer = _customerRepository.GetById(customerId);
            var plan = _subscriptionPlanRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }

            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
            string notes = string.Empty;

            var discountContext = new RenewalDiscountContext(
                customer,
                plan,
                seatCount,
                useLoyaltyPoints,
                baseAmount);

            var discountResult = _discountCalculator.Calculate(discountContext);

            decimal discountAmount = discountResult.Amount;
            notes += discountResult.Notes;

            decimal subtotalAfterDiscount = baseAmount - discountAmount;
            if (subtotalAfterDiscount < 300m)
            {
                subtotalAfterDiscount = 300m;
                notes += "minimum discounted subtotal applied; ";
            }

            decimal supportFee = 0m;
            if (includePremiumSupport)
            {
                supportFee = _supportFeeCalculator.Calculate(normalizedPlanCode);
                notes += "premium support included; ";
            }

            var paymentFeeContext = new PaymentFeeContext
            {
                SubTotalAfterDiscount = subtotalAfterDiscount,
                SupportFee = supportFee
            };

            var paymentFeeResult = _paymentFeeCalculator.Calculate(normalizedPaymentMethod, paymentFeeContext);
            decimal paymentFee = paymentFeeResult.Amount;
            notes += paymentFeeResult.Notes;

            decimal taxRate = _taxRateCalculator.Calculate(customer.Country);

            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxAmount = taxBase * taxRate;
            decimal finalAmount = taxBase + taxAmount;

            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                notes += "minimum invoice amount applied; ";
            }

            var invoice = new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(baseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(discountAmount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(supportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(paymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(taxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(finalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = notes.Trim(),
                GeneratedAt = DateTime.UtcNow
            };

            LegacyBillingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

                LegacyBillingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }
    }
}