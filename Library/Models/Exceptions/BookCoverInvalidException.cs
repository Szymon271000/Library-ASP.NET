using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Exceptions
{
    public class BookCoverInvalidException : Exception
    {
        public BookCoverInvalidException(int bookId, Exception innerException) : base($"Image for book '{bookId}' is not valid", innerException)
        {
        }
    }
}
