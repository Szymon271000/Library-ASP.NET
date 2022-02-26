using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Exceptions
{
    public class ImagePersistenceException : Exception
    {
        public ImagePersistenceException(Exception innerException) : base("Could not persist the image", innerException)
        {
        }
    }
}
