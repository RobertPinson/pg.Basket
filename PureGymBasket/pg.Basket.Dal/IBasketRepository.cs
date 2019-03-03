using System;
using System.Collections.Generic;
using System.Text;

namespace pg.Basket.Dal
{
    public interface IBasketRepository
    {
        Guid Add(Entity.Basket basket);
        Entity.Basket Get(Guid basketId);
    }
}
