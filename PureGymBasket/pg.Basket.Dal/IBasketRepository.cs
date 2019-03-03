using System;
using System.Collections.Generic;
using System.Text;

namespace pg.Basket.Dal
{
    public interface IBasketRepository
    {
        Entity.Basket Add(Entity.Basket basket);
        Entity.Basket Get(Guid basketId);
        Entity.Basket Update(Entity.Basket basket);
    }

    public class BasketRepository : IBasketRepository
    {
        private readonly BasketDbContext _dbContext;

        public BasketRepository(BasketDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Entity.Basket Add(Entity.Basket basket)
        {
            _dbContext.Baskets.Add(basket);
            _dbContext.SaveChanges();

            return basket; 
        }

        public Entity.Basket Get(Guid basketId)
        {
            throw new NotImplementedException();
        }

        public Entity.Basket Update(Entity.Basket basket)
        {
            throw new NotImplementedException();
        }
    }
}
