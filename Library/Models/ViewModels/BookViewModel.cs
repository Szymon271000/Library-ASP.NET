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
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BookCover { get; set; }
        public string Author { get; set; }
        public double Rating { get; set; }
        public Money FullPrice { get; set; }
        public Money CurrentPrice { get; set; }
        public static BookViewModel FromDataRow(DataRow dataRow)
        {
            var bookViewModel = new BookViewModel
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
                    Convert.ToDecimal(dataRow["CurrentPrice_Amount"]))
            };
            return bookViewModel;
        }
        public static BookViewModel FromEntity(Book book)
        {
            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                BookCover = book.BookCover,
                Author = book.Author,
                Rating = book.Rating,
                CurrentPrice = book.CurrentPrice,
                FullPrice = book.FullPrice
            };
        }
    }
}
