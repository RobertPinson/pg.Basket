using Microsoft.EntityFrameworkCore;
using pg.Basket.Model;

namespace pg.Basket.Dal
{
    public interface IBasketDbContext
    {
        DbSet<Model.Basket> Baskets { get; set; }
        DbSet<BasketItem> BasketItems { get; set; }
        DbSet<BaseVoucher> Vouchers { get; set; }
    }
}
