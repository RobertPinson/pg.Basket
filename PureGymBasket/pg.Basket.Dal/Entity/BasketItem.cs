using System;

namespace pg.Basket.Dal.Entity
{
    public class BasketItem
    {
        private bool _isDiscountable;
        private int _id;
        private string _description;
        private int _category;
        private readonly decimal _unitPrice;
        private int _quantity;

        public BasketItem(int id, string description, int category, decimal unitPrice, bool isDiscountable, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new Exception("Invalid quantity");
            }

            _id = id;
            _description = description;
            _category = category;
            _unitPrice = unitPrice;
            _quantity = quantity;
            _isDiscountable = isDiscountable;
        }

        public int GetBasketItemId()
        {
            return _id;
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

        public int GetCategory()
        {
            return _category;
        }

        public bool IsDiscountable()
        {
            return _isDiscountable;
        }
    }
}