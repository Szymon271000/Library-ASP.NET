using Library.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.ViewModels
{
    public class ChapterViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PublicationYear { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public static ChapterViewModel FromDataRow(DataRow dataRow)
        {
            var chapterViewModel = new ChapterViewModel
            {
                Id = Convert.ToInt32(dataRow["Id"]),
                Title = Convert.ToString(dataRow["Title"])
            };
            return chapterViewModel;
        }
        public static ChapterViewModel FromEntity(Chapter chapter)
        {
            var chapterViewModel = new ChapterViewModel
            {
                Id = chapter.Id,
                Title = chapter.Title
            };
            return chapterViewModel;
        }
    }
}
