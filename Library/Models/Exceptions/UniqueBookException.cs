using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Exceptions
{
    public class UniqueBookException : Exception
    {
        public UniqueBookException(string title, string author, Exception innerException) : base($"The book {title} by {author} is already present", innerException)
        {
        }
    }
}
