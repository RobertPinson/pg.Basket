namespace pg.Basket.Dal
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketDbContext _dbContext;

        public BasketRepository(BasketDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Model.Basket Add(Model.Basket basket)
        {
            _dbContext.Baskets.Add(basket);
            _dbContext.SaveChanges();

            return basket; 
        }

        public Model.Basket Get(int basketId)
        {
            return _dbContext.Baskets.Find(basketId);
        }

        public Model.Basket Update(Model.Basket basket)
        {
            _dbContext.Baskets.Update(basket);
            _dbContext.SaveChanges();

            return basket;
        }
    }
}