using Library.Models.InputModels;
using Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Services.Application
{
    public interface IChapterService
    {
        Task<ChapterDetailViewModel> GetChapterAsync(int id);
        Task<ChapterEditInputModel> GetChapterForEditingAsync(int id);
        Task<ChapterDetailViewModel> CreateChapterAsync(ChapterCreateInputModel inputModel);
        Task<ChapterDetailViewModel> EditChapterAsync(ChapterEditInputModel inputModel);
        Task DeleteChapterAsync(ChapterDeleteInputModel inputModel);
    }
}
