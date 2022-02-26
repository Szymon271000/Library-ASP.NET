using Library.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.ValueObjects
{
    public record Money
    {
        private decimal amount = 0;
        public Money() : this(Currency.EUR, 0.00m)
        {
        }
        public Money(Currency currency, decimal amount)
        {
            Amount = amount;
            Currency = currency;
        }
        public decimal Amount
        {
            get
            {
                return amount;
            }
            init
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("The amount can't be negative");
                }
                amount = value;
            }
        }
        public Currency Currency
        {
            get; init;
        }
        public override string ToString()
        {
            return $"{Currency} {Amount:0.00}";
        }
    }
}
