using pg.Basket.Bll.Interface;
using pg.Basket.Dal;
using pg.Basket.Model;

namespace pg.Basket.Bll.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public Model.Basket CreateBasket()
        {
            var basket = new Model.Basket();

            _basketRepository.Add(basket);
            return basket;
        }

        public Model.Basket AddItem(int basketId, BasketItem basketItem)
        {
            var basket = _basketRepository.Get(basketId);
            basket.AddBasketItem(basketItem);
            return basket;
        }

        public Model.Basket RemoveItem(int basketId, BasketItem basketItem)
        {
            var basket = _basketRepository.Get(basketId);
            basket.RemoveBasketItem(basketItem);
            _basketRepository.Update(basket);
            return basket;
        }

        public Model.Basket RedeemGiftVoucher(int basketId, GiftVoucher giftVoucher)
        {
            var basket = _basketRepository.Get(basketId);
            basket.AddVoucher(giftVoucher);
            return basket;
        }
    }
}
