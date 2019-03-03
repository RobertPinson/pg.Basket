using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using pg.Basket.Dal.Entity;

namespace pg.Basket.Bll.Interface
{
    interface IBasketService
    {
        Dal.Entity.Basket CreateBasket();
        Dal.Entity.Basket AddItem(Guid basketId, BasketItem basketItem);
        Dal.Entity.Basket RedeemGiftVoucher(Guid basketId, GiftVoucher giftVoucher);
    }
}
