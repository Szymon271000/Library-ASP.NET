using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Exceptions
{
    public class PaymentGatewayException : Exception
    {
        public PaymentGatewayException(Exception innerException) : base($"Payment gateway threw an exception", innerException)
        {
        }
    }
}
