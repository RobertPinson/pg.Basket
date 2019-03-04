using System;

namespace pg.Basket.Model
{
    public class BasketItem
    {
        private string _description;
        private int _categoryId;
        private decimal _unitPrice;
        private int _quantity;

        public int Id { get; set; }

        public int ProductId { get; private set; }

        protected BasketItem() { }

        public BasketItem(int productId, string description, int categoryId, decimal unitPrice, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new Exception("Invalid quantity");
            }

            ProductId = productId;

            _description = description;
            _categoryId = categoryId;
            _unitPrice = unitPrice;
            _quantity = quantity;
        }

        public int GetQuantity()
        {
            return _quantity;
        }

        public decimal GetUnitPrice()
        {
            return _unitPrice;
        }

        public void AddQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new Exception("Invalid quantity");
            }

            _quantity += quantity;
        }

        public int GetCategoryId()
        {
            return _categoryId;
        }
    }
}