using Library.Models.Entities;
using Library.Models.Exceptions;
using Library.Models.InputModels;
using Library.Models.Services.Database;
using Library.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Services.Application
{
    public class EfCoreChapterService : IChapterService
    {
        private readonly ILogger<EfCoreChapterService> logger;
        private readonly LibraryDbContext dbContext;
        public EfCoreChapterService(ILogger<EfCoreChapterService> logger, LibraryDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }
        public async Task<ChapterDetailViewModel> CreateChapterAsync(ChapterCreateInputModel inputModel)
        {
            var chapter = new Chapter(inputModel.Title, inputModel.BookId);
            dbContext.Add(chapter);
            await dbContext.SaveChangesAsync();

            return ChapterDetailViewModel.FromEntity(chapter);
        }

        public async Task DeleteChapterAsync(ChapterDeleteInputModel inputModel)
        {
            Chapter chapter = await dbContext.Chapters.FindAsync(inputModel.Id);
            if (chapter == null)
            {
                throw new ChapterNotFoundException(inputModel.Id);
            }
            dbContext.Remove(chapter);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ChapterDetailViewModel> EditChapterAsync(ChapterEditInputModel inputModel)
        {
            Chapter chapter = await dbContext.Chapters.FindAsync(inputModel.Id);

            if (chapter == null)
            {
                throw new ChapterNotFoundException(inputModel.Id);
            }

            chapter.ChangeTitle(inputModel.Title);
            chapter.ChangeOrder(inputModel.Order);

            await dbContext.SaveChangesAsync();
            return ChapterDetailViewModel.FromEntity(chapter);
        }

        public async Task<ChapterDetailViewModel> GetChapterAsync(int id)
        {
            IQueryable<ChapterDetailViewModel> queryLinq = dbContext.Chapters
                .AsNoTracking()
                .Where(chapter => chapter.Id == id)
                .Select(chapter => ChapterDetailViewModel.FromEntity(chapter));

            ChapterDetailViewModel viewModel = await queryLinq.FirstOrDefaultAsync();

            if (viewModel == null)
            {
                logger.LogWarning("Chapter {id} not found", id);
                throw new ChapterNotFoundException(id);
            }

            return viewModel;
        }

        public async Task<ChapterEditInputModel> GetChapterForEditingAsync(int id)
        {
            IQueryable<ChapterEditInputModel> queryLinq = dbContext.Chapters
                .AsNoTracking()
                .Where(chapter => chapter.Id == id)
                .Select(chapter => ChapterEditInputModel.FromEntity(chapter));

            ChapterEditInputModel inputModel = await queryLinq.FirstOrDefaultAsync();

            if (inputModel == null)
            {
                logger.LogWarning("Chapter {id} not found", id);
                throw new ChapterNotFoundException(id);
            }

            return inputModel;
        }
    }
}
