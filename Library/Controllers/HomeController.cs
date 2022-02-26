using Library.Models;
using Library.Models.Services.Application;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index([FromServices] IBookService bookService)
        {
            ViewData["Title"] = "Welcome to Library!";
            List<BookViewModel> bestRatingBooks = await bookService.GetBestRatingBooksAsync();
            List<BookViewModel> mostRecentBooks = await bookService.GetMostRecentBooksAsync();
            HomeViewModel viewModel = new HomeViewModel
            {
                BestRatingBooks = bestRatingBooks,
                MostRecentBooks = mostRecentBooks
            };
            return View(viewModel);
        }
    }
}
