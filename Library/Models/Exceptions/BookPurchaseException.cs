using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Exceptions
{
    public class BookPurchaseException : Exception
    {
        public BookPurchaseException(int bookId) : base($"Could not purchase book {bookId}")
        {

        }
    }
}
