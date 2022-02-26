using Library.Controllers;
using Library.Models.Entities;
using Library.Models.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    public class BookEditInputModel : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is mandatory"),
        MinLength(5, ErrorMessage = "The title should contain a minimum of {1} characters"),
        MaxLength(100, ErrorMessage = "The title should contain a maximum of {1} characters"),
        RegularExpression(@"^[\w\s\.\']+$", ErrorMessage = "The title is not valid. Only letters, numbers, spaces, dots and apostrophes allowed"),
        Remote(action: nameof(BooksController.IsBookUnique), controller: "Books", AdditionalFields = "Author,Id", ErrorMessage = "This book is already present"),
        Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Book Cover")]
        public string BookCover { get; set; }

        [Required(ErrorMessage = "The author is mandatory"),
        RegularExpression(@"^[\w\s\.]+$", ErrorMessage = "The author is not valid. Only letters, numbers, spaces and dots allowed"),
        Display(Name = "Author")]
        public string Author { get; set; }

        [Required(ErrorMessage = "The full price is mandatory"),
        Display(Name = "Full price")]
        public Money FullPrice { get; set; }

        [Required(ErrorMessage = "The current price is mandatory"),
        Display(Name = "Current price")]
        public Money CurrentPrice { get; set; }

        [MinLength(20, ErrorMessage = "The summary should contain a minimum of {1} characters"),
        MaxLength(8000, ErrorMessage = "The summary should contain a maximum of {1} characters")]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Display(Name = "New image...")]
        public IFormFile Image { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FullPrice.Currency != CurrentPrice.Currency)
            {
                yield return new ValidationResult("Full price should have the same currency as current price");
            }
            else if (FullPrice.Amount < CurrentPrice.Amount)
            {
                yield return new ValidationResult("Full price could not be less than current price");
            }
        }
        public static BookEditInputModel FromEntity(Book book)
        {
            return new BookEditInputModel
            {
                Id = book.Id,
                Title = book.Title,
                Summary = book.Summary,
                Author = book.Author,
                BookCover = book.BookCover,
                CurrentPrice = book.CurrentPrice,
                FullPrice = book.FullPrice
            };
        }
    }
}
