using Library.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    public class ChapterEditInputModel
    {
        [Required]
        public int Id { get; set; }

        public int BookId { get; set; }

        [Required(ErrorMessage = "The title is mandatory"),
        MinLength(5, ErrorMessage = "The title should contain a minimum of {1} characters"),
        MaxLength(100, ErrorMessage = "The title should contain a maximum of {1} characters"),
        Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Order"),
        Required(ErrorMessage = "Order is mandatory")]
        public int Order { get; set; }


        public static ChapterEditInputModel FromDataRow(DataRow bookRow)
        {
            var chapterEditInputModel = new ChapterEditInputModel
            {
                Id = Convert.ToInt32(bookRow["Id"]),
                BookId = Convert.ToInt32(bookRow["BookId"]),
                Title = Convert.ToString(bookRow["Title"]),
                Order = Convert.ToInt32(bookRow["Order"])
            };
            return chapterEditInputModel;
        }

        public static ChapterEditInputModel FromEntity(Chapter chapter)
        {
            return new ChapterEditInputModel
            {
                Id = chapter.Id,
                BookId = chapter.BookId,
                Title = chapter.Title,
                Order = chapter.Order
            };
        }
    }
}
