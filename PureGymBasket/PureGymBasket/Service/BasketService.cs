using System;
using System.Threading.Tasks;
using pg.Basket.Bll.Interface;
using pg.Basket.Dal;
using pg.Basket.Dal.Entity;

namespace pg.Basket.Bll.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public Dal.Entity.Basket CreateBasket()
        {
            var basket = new Dal.Entity.Basket();

            _basketRepository.Add(basket);
            return basket;
        }

        public Dal.Entity.Basket AddItem(Guid basketId, BasketItem basketItem)
        {
            var basket = _basketRepository.Get(basketId);
            basket.AddBasketItem(basketItem);
            return basket;
        }

        public Dal.Entity.Basket RedeemGiftVoucher(Guid basketId, GiftVoucher giftVoucher)
        {
            var basket = _basketRepository.Get(basketId);
            basket.AddGiftVoucher(giftVoucher);
            return basket;
        }
    }
}
