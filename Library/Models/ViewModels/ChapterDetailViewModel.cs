using Library.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.ViewModels
{
    public class ChapterDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PublicationYear { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public int BookId { get; set; }

        public static ChapterDetailViewModel FromDataRow(DataRow dataRow)
        {
            var chapterViewModel = new ChapterDetailViewModel
            {
                Id = Convert.ToInt32(dataRow["Id"]),
                BookId = Convert.ToInt32(dataRow["BookId"]),
                Title = Convert.ToString(dataRow["Title"])
            };
            return chapterViewModel;
        }

        public static ChapterDetailViewModel FromEntity(Chapter chapter)
        {
            return new ChapterDetailViewModel
            {
                Id = chapter.Id,
                BookId = chapter.BookId,
                Title = chapter.Title
            };
        }
    }
}
