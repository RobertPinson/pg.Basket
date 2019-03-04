using System.Collections.Generic;
using System.Linq;

namespace pg.Basket.Model
{
    public class OfferVoucher : BaseVoucher
    {
        public decimal MinSpend { get; }
        public List<int> OfferCategoryIds { get; }

        protected OfferVoucher()
        { }

        public OfferVoucher(string code, string description, decimal value, decimal minSpend, List<int> offerCategoryIds = null) : base(code, description, value)
        {
            MinSpend = minSpend;
            OfferCategoryIds = offerCategoryIds ?? new List<int>();
        }

        public override bool Validate(Basket basket)
        {
            if (basket.Vouchers.OfType<OfferVoucher>().Any())
            {
                basket.SetVoucherNotAppliedMessage("A voucher has already been applied to you basket.");
                return false;
            }

            //Gift voucher product category ProductId = 2
            //TODO Fix magic category number
            var discountableBasketTotal = basket.BasketItems.Where(i => i.GetCategoryId() != 2)
                .Sum(i => i.GetQuantity() * i.GetUnitPrice());

            if (discountableBasketTotal < MinSpend)
            {
                var shortfall = MinSpend - discountableBasketTotal + 0.01m;
                basket.SetVoucherNotAppliedMessage(
                    $"You have not reached the spend threshold for voucher {Code}. Spend another {shortfall:C} to receive {Value:C} discount from your basket total.");

                return false;
            }

            if (OfferCategoryIds.Any())
            {
                var validItems = basket.BasketItems
                    .Where(i => OfferCategoryIds.Contains(i.GetCategoryId()))
                    .ToList();

                if (!validItems.Any())
                {
                    basket.SetVoucherNotAppliedMessage(
                        $"There are no products in your basket applicable to voucher Voucher {Code}.");
                    return false;
                }
            }

            return true;
        }

        public override decimal CalculateDiscount(Basket basket)
        {
            decimal result;

            var discountableBasketTotal = basket.BasketItems.Where(i => i.GetCategoryId() != 2).Sum(i => i.GetQuantity() * i.GetUnitPrice());

            if (OfferCategoryIds.Any())
            {
                var validItems = basket.BasketItems.Where(i => OfferCategoryIds.Contains(i.GetCategoryId())).ToList();

                var discountableTotal = validItems.Sum(i => i.GetQuantity() * i.GetUnitPrice());
                result = Value >= discountableTotal
                    ? discountableTotal
                    : Value;
            }
            else
            {
                result = Value;
            }

            return result;
        }
    }
}