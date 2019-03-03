using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using pg.Basket.Dal.Entity;

namespace pg.Basket.Dal
{
    public interface IBasketDbContext
    {
        DbSet<Entity.Basket> Baskets { get; set; }
        DbSet<BasketItem> BasketItems { get; set; }
        DbSet<BaseVoucher> Vouchers { get; set; }
    }
}
