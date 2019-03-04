using System;
using System.Collections.Generic;
using System.Linq;

namespace pg.Basket.Model
{
    public class Basket
    {
        public int Id { get; set; }

        private readonly List<BasketItem> _basketItems;
        public IReadOnlyCollection<BasketItem> BasketItems => _basketItems;

        private readonly List<BaseVoucher> _vouchers;
        public IReadOnlyCollection<BaseVoucher> Vouchers => _vouchers;

        public string VoucherNotAppliedMessage { get; private set; }

        public Basket()
        {
            _basketItems = new List<BasketItem>();
            _vouchers = new List<BaseVoucher>();
        }

        #region Public Methods

        public void AddBasketItem(BasketItem newItem)
        {
            var existingItem = _basketItems.SingleOrDefault(i => i.ProductId == newItem.ProductId);

            if (existingItem != null)
                existingItem.AddQuantity(newItem.GetQuantity());
            else
            {
                _basketItems.Add(newItem);
            }

            ValidateBasket();
        }

        public void RemoveBasketItem(BasketItem item)
        {
            var existingItem = _basketItems.SingleOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null && existingItem.GetQuantity() > 1)
                existingItem.AddQuantity(-1);

            ValidateBasket();
        }

        public bool AddVoucher(BaseVoucher voucher)
        {
            if (!voucher.Validate(this)) return false;

            _vouchers.Add(voucher);
            return true;
        }

        public decimal GetSubTotal()
        {
            var runningTotal = GetBasketTotal();
            if (_vouchers.Count == 0)
            {
                return runningTotal;
            }

            var firstGiftVoucher = _vouchers.OfType<GiftVoucher>().FirstOrDefault();
            if (firstGiftVoucher != null)
            {
                var giftVoucherTotalDiscount = firstGiftVoucher.CalculateDiscount(this);
                runningTotal = runningTotal - giftVoucherTotalDiscount;
            }

            var singleOfferVoucher = _vouchers.OfType<OfferVoucher>().SingleOrDefault();
            if (singleOfferVoucher != null)
            {
                var offerVoucherDiscountTotal = singleOfferVoucher.CalculateDiscount(this);
                runningTotal = runningTotal - offerVoucherDiscountTotal;
            }

            return Math.Round(runningTotal, 2);
        }

        public decimal GetBasketTotal()
        {
            if (_basketItems.Count <= 0)
            {
                return 0;
            }

            return _basketItems.Sum(i => i.GetQuantity() * i.GetUnitPrice());
        }

        public void SetVoucherNotAppliedMessage(string message)
        {
            VoucherNotAppliedMessage = message;
        }

        #endregion

        #region Private Methods

        private void ValidateBasket()
        {
            foreach (var voucher in Vouchers)
            {
                voucher.Validate(this);
            }
        }

        #endregion
    }
}
