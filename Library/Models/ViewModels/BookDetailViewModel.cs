using Library.Models.Entities;
using Library.Models.Enums;
using Library.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.ViewModels
{
    public class BookDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BookCover { get; set; }
        public string Author { get; set; }
        public double Rating { get; set; }
        public Money FullPrice { get; set; }
        public Money CurrentPrice { get; set; }
        public string Summary { get; set; }
        public List<ChapterViewModel> Chapters { get; set; } = new List<ChapterViewModel>();

        public static BookDetailViewModel FromDataRow(DataRow dataRow)
        {
            var bookDetailViewModel = new BookDetailViewModel
            {
                Id = Convert.ToInt32(dataRow["Id"]),
                Title = Convert.ToString(dataRow["Title"]),
                BookCover = Convert.ToString(dataRow["BookCover"]),
                Author = Convert.ToString(dataRow["Author"]),
                Rating = Convert.ToDouble(dataRow["Rating"]),
                FullPrice = new Money(
                        Enum.Parse<Currency>(Convert.ToString(dataRow["FullPrice_Currency"])),
                        Convert.ToDecimal(dataRow["FullPrice_Amount"])),
                CurrentPrice = new Money(
                        Enum.Parse<Currency>(Convert.ToString(dataRow["CurrentPrice_Currency"])),
                        Convert.ToDecimal(dataRow["CurrentPrice_Amount"])),
                Summary = Convert.ToString(dataRow["Summary"]),
                Chapters = new List<ChapterViewModel>()
            };
            return bookDetailViewModel;
        }
        public static BookDetailViewModel FromEntity(Book book)
        {
            return new BookDetailViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Summary = book.Summary,
                Author = book.Author,
                BookCover = book.BookCover,
                Rating = book.Rating,
                CurrentPrice = book.CurrentPrice,
                FullPrice = book.FullPrice,
                Chapters = book.Chapters
                                    .OrderBy(chapter => chapter.Order)
                                    .ThenBy(chapter => chapter.Id)
                                    .Select(chapter => ChapterViewModel.FromEntity(chapter))
                                    .ToList()
            };
        }
    }
}
