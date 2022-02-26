using Library.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    public class BookPurchaseInputModel
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime PaymentDate { get; set; }
        public Money Paid { get; set; }
        public string TransactionId { get; set; }
    }
}
