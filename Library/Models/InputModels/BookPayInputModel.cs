using Library.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    public class BookPayInputModel
    {
        public int BookId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public Money Price { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
