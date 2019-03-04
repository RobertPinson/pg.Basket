using System.Collections.Generic;
using NUnit.Framework;
using pg.Basket.Model;

namespace pg.Basket.UnitTests
{
    public class BasketModelTests
    {
        private const int ProductCategoryGiftCard = 2;
        private const int ProductCategoryHat = 10;
        private const int ProductCategoryTops = 20;
        private const int ProductCategoryHeadGear = 30;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GiftVoucher_Redeemed_OK()
        {
            //Arrange
            var basket = new Model.Basket();

            var giftVoucher = new GiftVoucher("111-222", "£5.00 Gift Voucher", 5.00m);

            var hat = new BasketItem(1, "Hat", ProductCategoryHat, 10.50m);
            basket.AddBasketItem(hat);

            var jumper = new BasketItem(2, "Jumper", ProductCategoryTops, 54.65m);
            basket.AddBasketItem(jumper);

            //Act
            var addVoucherResult = basket.AddVoucher(giftVoucher);

            //Assert
            Assert.AreEqual(addVoucherResult, true);
            Assert.AreEqual(basket.BasketItems.Count, 2);
            Assert.AreEqual(basket.Vouchers.Count, 1);
            Assert.That(basket.VoucherNotAppliedMessage, Is.Null);
            Assert.AreEqual(basket.GetSubTotal(), 60.15m);
        }

        [Test]
        public void OfferVoucher_NotRedeemed_InvalidBasketItems()
        {
            //Arrange
            var basket = new Model.Basket();

            var hat = new BasketItem(1, "Hat", ProductCategoryHat, 25.00m);
            basket.AddBasketItem(hat);

            var jumper = new BasketItem(2, "Jumper", ProductCategoryTops, 26.00m);
            basket.AddBasketItem(jumper);

            var headGearVoucher = new OfferVoucher("111-333", "£5.00 off Head Gear in baskets over £50.00 Offer Voucher", 5.00m, 50.00m, new List<int> {ProductCategoryHeadGear});

            //Act
            var addVoucherResult = basket.AddVoucher(headGearVoucher);

            //Assert
            Assert.AreEqual(addVoucherResult, false);
            Assert.AreEqual(basket.BasketItems.Count, 2);
            Assert.AreEqual(basket.Vouchers.Count, 0);
            Assert.That(basket.VoucherNotAppliedMessage, Is.Not.Null);
            Assert.AreEqual(basket.VoucherNotAppliedMessage, "There are no products in your basket applicable to voucher Voucher 111-333.");
            Assert.AreEqual(basket.GetSubTotal(), 51.00m);
        }

        [Test]
        public void OfferVoucher_PartRedeemed_OK()
        {
            //Arrange
            var basket = new Model.Basket();

            var hat = new BasketItem(1, "Hat", ProductCategoryHat, 25.00m);
            basket.AddBasketItem(hat);

            var jumper = new BasketItem(2, "Jumper", ProductCategoryTops, 26.00m);
            basket.AddBasketItem(jumper);

            var headLight = new BasketItem(3, "Head Light", ProductCategoryHeadGear, 3.50m);
            basket.AddBasketItem(headLight);

            var headGearVoucher = new OfferVoucher("111-333", "£5.00 off Head Gear in baskets over £50.00 Offer Voucher", 5.00m, 50.00m, new List<int> {ProductCategoryHeadGear});

            //Act
            var addVoucherResult = basket.AddVoucher(headGearVoucher);

            //Assert
            Assert.AreEqual(addVoucherResult, true);
            Assert.AreEqual(basket.BasketItems.Count, 3);
            Assert.AreEqual(basket.Vouchers.Count, 1);
            Assert.That(basket.VoucherNotAppliedMessage, Is.Null);
            Assert.AreEqual(basket.GetSubTotal(), 51.00m);
        }

        [Test]
        public void OfferGiftVoucher_Redeemed_OK()
        {
            //Arrange
            var basket = new Model.Basket();

            var hat = new BasketItem(1, "Hat", ProductCategoryHat, 25.00m);
            basket.AddBasketItem(hat);

            var jumper = new BasketItem(2, "Jumper", ProductCategoryTops, 26.00m);
            basket.AddBasketItem(jumper);

            var offerVoucher = new OfferVoucher("111-333", "£5.00 off baskets over £50.00 Offer Voucher", 5.00m, 50.00m);
            var giftVoucher = new GiftVoucher("111-222", "£5.00 Gift Voucher", 5.00m);

            //Act
            var addGiftVoucherResult = basket.AddVoucher(giftVoucher);
            var addOfferVoucherResult = basket.AddVoucher(offerVoucher);

            //Assert
            Assert.AreEqual(addGiftVoucherResult, true);
            Assert.AreEqual(addOfferVoucherResult, true);
            Assert.AreEqual(basket.BasketItems.Count, 2);
            Assert.AreEqual(basket.Vouchers.Count, 2);
            Assert.That(basket.VoucherNotAppliedMessage, Is.Null);
            Assert.AreEqual(basket.GetSubTotal(), 41.00m);
        }

        [Test]
        public void GiftVoucherProduct_NotIncludedDiscountableTotal()
        {
            //Arrange
            var basket = new Model.Basket();

            var hat = new BasketItem(1, "Hat", ProductCategoryHat, 25.00m);
            basket.AddBasketItem(hat);

            var jumper = new BasketItem(4, "£30 Gift Voucher", ProductCategoryGiftCard, 30.00m);
            basket.AddBasketItem(jumper);

            var offerVoucher = new OfferVoucher("111-333", "£5.00 off baskets over £50.00 Offer Voucher", 5.00m, 50.00m);

            //Act
            var addOfferVoucherResult = basket.AddVoucher(offerVoucher);

            //Assert
            Assert.AreEqual(addOfferVoucherResult, false);
            Assert.AreEqual(basket.BasketItems.Count, 2);
            Assert.AreEqual(basket.Vouchers.Count, 0);
            Assert.That(basket.VoucherNotAppliedMessage, Is.Not.Null);
            Assert.AreEqual(basket.VoucherNotAppliedMessage, "You have not reached the spend threshold for voucher 111-333. Spend another £25.01 to receive £5.00 discount from your basket total.");
            Assert.AreEqual(basket.GetSubTotal(), 55.00m);
        }
    }
}