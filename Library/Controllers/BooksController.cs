using Library.Models.Enums;
using Library.Models.Exceptions;
using Library.Models.InputModels;
using Library.Models.Services.Application;
using Library.Models.ValueObjects;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(BookListInputModel input)
        {
            ListViewModel<BookViewModel> books = await bookService.GetBooksAsync(input);
            ViewBag.Title = "Books";
            BookListViewModel viewModel = new BookListViewModel
            {
                Books = books,
                Input = input
            };
            return View(viewModel);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Detail(int id)
        {
            BookDetailViewModel viewModel = await bookService.GetBookAsync(id);
            ViewBag.Title = viewModel.Title;
            return View(viewModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        public IActionResult Create()
        {
            ViewData["Title"] = "New book";
            var inputModel = new BookCreateInputModel();
            return View(inputModel);
        }

        public async Task<IActionResult> Pay(int id)
        {
            string paymentUrl = await bookService.GetPaymentUrlAsync(id);
            return Redirect(paymentUrl);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Purchase(int id, string token)
        {
            BookPurchaseInputModel inputModel = await bookService.CapturePaymentAsync(id, token);
            await bookService.PurchaseBookAsync(inputModel);
            TempData["ConfirmationMessage"] = "Thank you for having purchased this book";
            return RedirectToAction(nameof(Detail), new { id = id });
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BookDetailViewModel book = await bookService.CreateBookAsync(inputModel);
                    TempData["ConfirmationMessage"] = "Your book has been created. Would you like to complete the info?";
                    return RedirectToAction(nameof(Edit), new { id = book.Id }); ;
                }
                catch (UniqueBookException)
                {
                    ModelState.AddModelError(nameof(BookDetailViewModel.Title), "This book is already present");
                    ModelState.AddModelError(nameof(BookDetailViewModel.Author), String.Empty);
                }
            } 
            ViewData["Title"] = "New book";
            return View(inputModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> IsBookUnique(string title, string author, int id = 0)
        {
            bool result = await bookService.IsBookUniqueAsync(title, author, id);
            return Json(result);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit book";
            BookEditInputModel inputModel = await bookService.GetBookForEditingAsync(id);
            return View(inputModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpPost]
        public async Task<IActionResult> Edit(BookEditInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BookDetailViewModel book = await bookService.EditBookAsync(inputModel);
                    TempData["ConfirmationMessage"] = "All the changes have been saved";
                    return RedirectToAction(nameof(Detail), new { id = inputModel.Id });
                }
                catch (UniqueBookException)
                {
                    ModelState.AddModelError(nameof(BookEditInputModel.Title), "This book is already present");
                    ModelState.AddModelError(nameof(BookEditInputModel.Author), String.Empty);
                }
                catch (BookCoverInvalidException)
                {
                    ModelState.AddModelError(nameof(BookEditInputModel.Image), "The selected image is not valid");
                }
            }
            ViewData["Title"] = "Edit book";
            return View(inputModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpPost]
        public async Task<IActionResult> Delete(BookDeleteInputModel inputModel)
        {
            await bookService.DeleteBookAsync(inputModel);
            TempData["ConfirmationMessage"] = "The book has been deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
