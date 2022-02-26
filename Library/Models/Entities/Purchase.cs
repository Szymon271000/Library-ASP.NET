using Library.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Entities
{
    public class Purchase
    {
        public Purchase(string userId, int bookId)
        {
            UserId = userId;
            BookId = bookId;
        }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public Money Paid { get; set; }
        public string TransactionId { get; set; }
        public int? Vote { get; set; }
        public virtual Book Book { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
