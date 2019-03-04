namespace pg.Basket.Model
{
    public abstract class BaseVoucher
    {
        public int Id { get; set; }
        public string Code { get; }
        public string Description { get; }
        public decimal Value { get; }

        protected BaseVoucher(string code, string description, decimal value)
        {
            Code = code;
            Description = description;
            Value = value;
        }

        protected BaseVoucher()
        {
        }

        public abstract bool Validate(Basket basket);
        public abstract decimal CalculateDiscount(Basket basket);

        public virtual Basket Basket { get; set; }
    }
}