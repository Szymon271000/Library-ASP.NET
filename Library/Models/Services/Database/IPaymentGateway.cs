using Library.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Services.Database
{
    public interface IPaymentGateway
    {
        Task<string> GetPaymentUrlAsync(BookPayInputModel inputModel);
        Task<BookPurchaseInputModel> CapturePaymentAsync(string token);
    }
}
