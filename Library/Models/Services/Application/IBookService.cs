using Library.Models.InputModels;
using Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Services.Application
{
    public interface IBookService
    {
        Task<ListViewModel<BookViewModel>> GetBooksAsync(BookListInputModel model);
        Task<BookDetailViewModel> GetBookAsync(int id);
        Task<List<BookViewModel>> GetBestRatingBooksAsync();
        Task<List<BookViewModel>> GetMostRecentBooksAsync();
        Task<BookEditInputModel> GetBookForEditingAsync(int id);
        Task<BookDetailViewModel> CreateBookAsync(BookCreateInputModel inputModel);
        Task<BookDetailViewModel> EditBookAsync(BookEditInputModel inputModel);
        Task DeleteBookAsync(BookDeleteInputModel inputModel);
        Task PurchaseBookAsync(BookPurchaseInputModel inputModel);
        Task<bool> IsBookUniqueAsync(string title, string author, int id);
        Task<bool> IsBookPurchasedAsync(int bookId, string userId);
        Task<string> GetPaymentUrlAsync(int bookId);
        Task<BookPurchaseInputModel> CapturePaymentAsync(int bookId, string token);
    }
}
