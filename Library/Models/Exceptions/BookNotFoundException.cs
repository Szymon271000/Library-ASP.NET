using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(int bookId) : base($"Book {bookId} not found")
        {
        }
    }
}
