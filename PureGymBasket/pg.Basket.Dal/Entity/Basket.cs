using System;
using System.Collections.Generic;
using System.Linq;

namespace pg.Basket.Dal.Entity
{
    public class Basket
    {
        private readonly Guid _id;
        private readonly List<BasketItem> _basketItems;
        public IReadOnlyCollection<BasketItem> BasketItems => _basketItems;
        private readonly List<BaseVoucher> _vouchers;
        public IReadOnlyCollection<BaseVoucher> Vouchers => _vouchers;

        public string VoucherNotAppliedMessage { get; private set; }

        public Basket()
        {
            _id = new Guid();
            _basketItems = new List<BasketItem>();
            _vouchers = new List<BaseVoucher>();
        }

        #region Public Methods

        public void AddBasketItem(BasketItem newItem)
        {
            var existingItem = _basketItems.SingleOrDefault(i => i.GetBasketItemId() == newItem.GetBasketItemId());

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
            var existingItem = _basketItems.SingleOrDefault(i => i.GetBasketItemId() == item.GetBasketItemId());

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

        public Guid GetId()
        {
            return _id;
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

    public abstract class BaseVoucher
    {
        public string Code { get; }
        public string Description { get; }
        public decimal Value { get; }

        protected BaseVoucher(string code, string description, decimal value)
        {
            Code = code;
            Description = description;
            Value = value;
        }

        public abstract bool Validate(Basket basket);
        public abstract decimal CalculateDiscount(Basket basket);
    }

    public class GiftVoucher : BaseVoucher
    {
        public GiftVoucher(string code, string description, decimal value)
            : base(code, description, value)
        {
        }

        public override bool Validate(Basket basket)
        {
            //TODO add any Gift voucher validation code here
            return true;
        }

        public override decimal CalculateDiscount(Basket basket)
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

    public class OfferVoucher : BaseVoucher
    {
        public decimal MaxSpend { get; }
        public int[] OfferProductIds { get; }

        public OfferVoucher(string code, string description, decimal value, decimal maxSpend, int[] offerProductIds) : base(code, description, value)
        {
            MaxSpend = maxSpend;
            OfferProductIds = offerProductIds;
        }

        public override bool Validate(Basket basket)
        {
            if (basket.Vouchers.OfType<OfferVoucher>().Any())
            {
                basket.SetVoucherNotAppliedMessage("A voucher has already been applied to you basket.");
                return false;
            }

            //Gift voucher product category Id = 2
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
                .Where(i => OfferProductIds.Contains(i.GetBasketItemId()))
                .ToList();

            if (!validItems.Any())
            {
                basket.SetVoucherNotAppliedMessage(
                    $"There are no products in your basket applicable to voucher Voucher {this.Code}.");
                return false;
            }

            return true;
        }

        public override decimal CalculateDiscount(Basket basket)
        {
            if (!this.Validate(basket))
            {
                return 0;
            }

            decimal result = 0;

            var discountableBasketTotal = basket.BasketItems.Where(i => i.GetCategory() != 2).Sum(i => i.GetQuantity() * i.GetUnitPrice());

            if (this.OfferProductIds.Length > 0)
            {
                var validItems = basket.BasketItems.Where(i => this.OfferProductIds.Contains(i.GetBasketItemId())).ToList();

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
