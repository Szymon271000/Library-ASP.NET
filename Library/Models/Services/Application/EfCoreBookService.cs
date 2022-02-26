using Library.Controllers;
using Library.Models.Entities;
using Library.Models.Enums;
using Library.Models.Exceptions;
using Library.Models.InputModels;
using Library.Models.Options;
using Library.Models.Services.Database;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Models.Services.Application
{
    public class EfCoreBookService : IBookService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<EfCoreBookService> logger;
        private readonly LibraryDbContext dbContext;
        private readonly IBookCoverPersister bookCoverPersister;
        private readonly IOptionsMonitor<BooksOptions> booksOptions;
        private readonly IPaymentGateway paymentGateway;
        private readonly LinkGenerator linkGenerator;

        public EfCoreBookService(IHttpContextAccessor httpContextAccessor, ILogger<EfCoreBookService> logger, LibraryDbContext dbContext, IBookCoverPersister bookCoverPersister, IOptionsMonitor<BooksOptions> booksOptions, IPaymentGateway paymentGateway, LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.dbContext = dbContext;
            this.bookCoverPersister = bookCoverPersister;
            this.booksOptions = booksOptions;
            this.paymentGateway = paymentGateway;
            this.linkGenerator = linkGenerator;
        }
        public async Task<BookDetailViewModel> GetBookAsync(int id)
        {
            IQueryable<BookDetailViewModel> queryLinq = dbContext.Books
                .AsNoTracking()
                .Include(book => book.Chapters)
                .Where(book => book.Id == id)
                .Select(book => BookDetailViewModel.FromEntity(book));
            BookDetailViewModel viewModel = await queryLinq.FirstOrDefaultAsync();
            if (viewModel == null)
            {
                logger.LogWarning("Book {id} not found", id);
                throw new BookNotFoundException(id);
            }
            return viewModel;
        }
        public async Task<ListViewModel<BookViewModel>> GetBooksAsync(BookListInputModel model)
        {
            IQueryable<Book> baseQuery = dbContext.Books;
            switch (model.OrderBy)
            {
                case "Title":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(book => book.Title);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(book => book.Title);
                    }
                    break;
                case "Rating":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(book => book.Rating);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(book => book.Rating);
                    }
                    break;
                case "CurrentPrice":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(book => book.CurrentPrice.Amount);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(book => book.CurrentPrice.Amount);
                    }
                    break;
                case "Id":
                    if (model.Ascending)
                    {
                        baseQuery = baseQuery.OrderBy(book => book.Id);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(book => book.Id);
                    }
                    break;
            }
            IQueryable<BookViewModel> queryLinq = baseQuery
                .Where(book => book.Title.Contains(model.Search))
                .Select(book => BookViewModel.FromEntity(book))
                .AsNoTracking();
            List<BookViewModel> books = await queryLinq
                .Skip(model.Offset)
                .Take(model.Limit)
                .ToListAsync();
            int totalCount = await queryLinq.CountAsync();
            ListViewModel<BookViewModel> result = new ListViewModel<BookViewModel>
            {
                Results = books,
                TotalCount = totalCount
            };

            return result;
        }
        public async Task<List<BookViewModel>> GetBestRatingBooksAsync()
        {
            BookListInputModel inputModel = new BookListInputModel(
                search: "",
                page: 1,
                orderby: "Rating",
                ascending: false,
                limit: booksOptions.CurrentValue.InHome,
                orderOptions: booksOptions.CurrentValue.Order);

            ListViewModel<BookViewModel> result = await GetBooksAsync(inputModel);
            return result.Results;
        }
        public async Task<List<BookViewModel>> GetMostRecentBooksAsync()
        {
            BookListInputModel inputModel = new BookListInputModel(
                search: "",
                page: 1,
                orderby: "Id",
                ascending: false,
                limit: booksOptions.CurrentValue.InHome,
                orderOptions: booksOptions.CurrentValue.Order);

            ListViewModel<BookViewModel> result = await GetBooksAsync(inputModel);
            return result.Results;
        }
        public async Task<BookDetailViewModel> CreateBookAsync(BookCreateInputModel inputModel)
        {
            string title = inputModel.Title;
            string author = inputModel.Author;
            var book = new Book(title, author);
            dbContext.Add(book);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exc) when ((exc.InnerException as SqlException)?.ErrorCode == 19)
            {
                throw new UniqueBookException(title, author, exc);
            }
            return BookDetailViewModel.FromEntity(book);
        }
        public async Task<bool> IsBookUniqueAsync(string title, string author, int id)
        {
            bool bookExists = await dbContext.Books.AnyAsync(book => EF.Functions.Like(book.Title, title) && EF.Functions.Like(book.Author, author) && book.Id != id);
            return !bookExists;
        }
        public async Task<BookEditInputModel> GetBookForEditingAsync(int id)
        {
            IQueryable<BookEditInputModel> queryLinq = dbContext.Books
                .AsNoTracking()
                .Where(book => book.Id == id)
                .Select(book => BookEditInputModel.FromEntity(book));
            BookEditInputModel viewModel = await queryLinq.FirstOrDefaultAsync();
            if (viewModel == null)
            {
                logger.LogWarning("Book {id} not found", id);
                throw new BookNotFoundException(id);
            }
            return viewModel;
        }
        public async Task<BookDetailViewModel> EditBookAsync(BookEditInputModel inputModel)
        {
            Book book = await dbContext.Books.FindAsync(inputModel.Id);
            book.ChangeTitle(inputModel.Title);
            book.ChangeAuthor(inputModel.Author);
            book.ChangePrices(inputModel.FullPrice, inputModel.CurrentPrice);
            book.ChangeSummary(inputModel.Summary);
            if (inputModel.Image != null)
            {
                try
                {
                    string imagePath = await bookCoverPersister.SaveBookCoverAsync(inputModel.Id, inputModel.Image);
                    book.ChangeImagePath(imagePath);
                }
                catch (Exception exc)
                {
                    throw new BookCoverInvalidException(inputModel.Id, exc);
                }
            }
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (ConstraintViolationException exc)
            {
                throw new UniqueBookException(inputModel.Title, inputModel.Author, exc);
            }
            return BookDetailViewModel.FromEntity(book);
        }
        
        public async Task DeleteBookAsync(BookDeleteInputModel inputModel)
        {
            Book book = await dbContext.Books.FindAsync(inputModel.Id);
            if (book == null)
            {
                throw new BookNotFoundException(inputModel.Id);
            }
            book.ChangeStatus(BookStatus.Deleted);
            await dbContext.SaveChangesAsync();
        }

        public async Task PurchaseBookAsync(BookPurchaseInputModel inputModel)
        {
            Purchase purchase = new Purchase(inputModel.UserId, inputModel.BookId)
            {
                PaymentDate = inputModel.PaymentDate,
                Paid = inputModel.Paid,
                TransactionId = inputModel.TransactionId
            };
            dbContext.Purchases.Add(purchase);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new BookPurchaseException(inputModel.BookId);
            }
        }
        public Task<bool> IsBookPurchasedAsync(int bookId, string userId)
        {
            return dbContext.Purchases.Where(purchase => purchase.BookId == bookId && purchase.UserId == userId).AnyAsync();
        }

        public async Task<string> GetPaymentUrlAsync(int bookId)
        {
            BookDetailViewModel viewModel = await GetBookAsync(bookId);
            BookPayInputModel inputModel = new BookPayInputModel()
            {
                BookId = bookId,
                UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                Description = viewModel.Title,
                Price = viewModel.CurrentPrice,
                ReturnUrl = linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, action: nameof(BooksController.Purchase), controller: "Books", values: new { id = bookId }),
                CancelUrl = linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, action: nameof(BooksController.Detail), controller: "Books", values: new { id = bookId }),
            };
            return await paymentGateway.GetPaymentUrlAsync(inputModel);
        }

        public Task<BookPurchaseInputModel> CapturePaymentAsync(int bookId, string token)
        {
            return paymentGateway.CapturePaymentAsync(token);
        }
    }
}
