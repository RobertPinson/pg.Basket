using System.Linq;

namespace pg.Basket.Entity
{
    public class OfferVoucher : BaseVoucher
    {
        public decimal MaxSpend { get; }
        public int[] OfferProductIds { get; }

        protected OfferVoucher() : base() { }

        public OfferVoucher(string code, string description, decimal value, decimal maxSpend, int[] offerProductIds) : base(code, description, value)
        {
            MaxSpend = maxSpend;
            OfferProductIds = offerProductIds;
        }

        public override bool Validate(pg.Basket.Entity.Basket basket)
        {
            if (basket.Vouchers.OfType<OfferVoucher>().Any())
            {
                basket.SetVoucherNotAppliedMessage("A voucher has already been applied to you basket.");
                return false;
            }

            //Gift voucher product category ProductId = 2
            //TODO Fix magic category number
            var discountableBasketTotal = basket.BasketItems.Where(i => i.GetCategory() != 2)
                .Sum(i => i.GetQuantity() * i.GetUnitPrice());

            if (this.MaxSpend <= discountableBasketTotal)
            {
                var shortfall = this.MaxSpend - discountableBasketTotal + 1;
                basket.SetVoucherNotAppliedMessage(
                    $"You have not reached the spend maxSpend for voucher {this.Code}. Spend another {shortfall:C} to receive {this.Value:C} discount from your basket total.");

                return false;
            }

            var validItems = basket.BasketItems
                .Where(i => OfferProductIds.Contains(i.ProductId))
                .ToList();

            if (!validItems.Any())
            {
                basket.SetVoucherNotAppliedMessage(
                    $"There are no products in your basket applicable to voucher Voucher {this.Code}.");
                return false;
            }

            return true;
        }

        public override decimal CalculateDiscount(pg.Basket.Entity.Basket basket)
        {
            if (!this.Validate(basket))
            {
                return 0;
            }

            decimal result = 0;

            var discountableBasketTotal = basket.BasketItems.Where(i => i.GetCategory() != 2).Sum(i => i.GetQuantity() * i.GetUnitPrice());

            if (this.OfferProductIds.Length > 0)
            {
                var validItems = basket.BasketItems.Where(i => this.OfferProductIds.Contains(i.ProductId)).ToList();

                var discountableTotal = validItems.Sum(i => i.GetQuantity() * i.GetUnitPrice());
                result = this.Value >= discountableTotal
                    ? discountableTotal
                    : this.Value;
            }
            else
            {
                result = this.Value;
            }

            return result;
        }
    }
}