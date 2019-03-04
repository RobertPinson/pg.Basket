using pg.Basket.Model;

namespace pg.Basket.Bll.Interface
{
    interface IBasketService
    {
        Model.Basket CreateBasket();
        Model.Basket AddItem(int basketId, BasketItem basketItem);
        Model.Basket RemoveItem(int basketId, BasketItem basketItem);
        Model.Basket RedeemGiftVoucher(int basketId, GiftVoucher giftVoucher);
    }
}
