using Library.Models.Enums;
using Library.Models.InputModels;
using Library.Models.Services.Application;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly IChapterService chapterService;

        public ChaptersController(IChapterService chapterService)
        {
            this.chapterService = chapterService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detail(int id)
        {
            ChapterDetailViewModel viewModel = await chapterService.GetChapterAsync(id);
            ViewData["Title"] = viewModel.Title;
            return View(viewModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        public IActionResult Create(int id)
        {
            ViewData["Title"] = "New chapter";
            var inputModel = new ChapterCreateInputModel();
            inputModel.BookId = id;
            return View(inputModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpPost]
        public async Task<IActionResult> Create(ChapterCreateInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                ChapterDetailViewModel chapter = await chapterService.CreateChapterAsync(inputModel);
                TempData["ConfirmationMessage"] = "New chapter has been added";
                return RedirectToAction(nameof(Edit), new { id = chapter.Id });
            }

            ViewData["Title"] = "New chapter";
            return View(inputModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Edit chapter";
            ChapterEditInputModel inputModel = await chapterService.GetChapterForEditingAsync(id);
            return View(inputModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpPost]
        public async Task<IActionResult> Edit(ChapterEditInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                ChapterDetailViewModel viewModel = await chapterService.EditChapterAsync(inputModel);
                TempData["ConfirmationMessage"] = "All the changes have been saved";
                return RedirectToAction(nameof(Detail), new { id = viewModel.Id });
            }

            ViewData["Title"] = "Edit chapter";
            return View(inputModel);
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpPost]
        public async Task<IActionResult> Delete(ChapterDeleteInputModel inputModel)
        {
            await chapterService.DeleteChapterAsync(inputModel);
            TempData["ConfirmationMessage"] = "The chapter has been deleted";
            return RedirectToAction(nameof(BooksController.Detail), "Books", new { id = inputModel.BookId });
        }
    }
}
