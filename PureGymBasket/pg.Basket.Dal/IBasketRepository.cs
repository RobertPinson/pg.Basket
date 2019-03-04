namespace pg.Basket.Dal
{
    public interface IBasketRepository
    {
        Model.Basket Add(Model.Basket basket);
        Model.Basket Get(int basketId);
        Model.Basket Update(Model.Basket basket);
    }
}
