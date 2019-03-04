using System.Linq;

namespace pg.Basket.Entity
{
    public class GiftVoucher : BaseVoucher
    {
        protected GiftVoucher() : base()
        { }

        public GiftVoucher(string code, string description, decimal value)
            : base(code, description, value)
        {
        }

        public override bool Validate(pg.Basket.Entity.Basket basket)
        {
            //TODO add any Gift voucher validation code here
            return true;
        }

        public override decimal CalculateDiscount(pg.Basket.Entity.Basket basket)
        {
            decimal result = 0;

            //Product Category 2 = Gift Vouchers
            //TODO fix magic product category number
            var discountableBasketTotal = basket.BasketItems.Where(i => i.GetCategory() != 2).Sum(i => i.GetQuantity() * i.GetUnitPrice());

            if (basket.Vouchers.OfType<GiftVoucher>().Count() != 0)
            {
                var giftVoucherTotalDiscount = basket.Vouchers.OfType<GiftVoucher>().Sum(v => v.Value);
                var totalGiftVoucherDiscount = discountableBasketTotal >= giftVoucherTotalDiscount ? giftVoucherTotalDiscount : discountableBasketTotal;
                result = totalGiftVoucherDiscount;
            }

            return result;
        }
    }
}