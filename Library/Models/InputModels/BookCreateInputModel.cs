using Library.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    public class BookCreateInputModel
    {
        [Required(ErrorMessage = "The title is mandatory"),
        MinLength(5, ErrorMessage = "The title should contain a minimum of {1} characters"),
        MaxLength(100, ErrorMessage = "The title should contain a maximum of {1} characters"),
        RegularExpression(@"^[\w\s\.\']+$", ErrorMessage = "The title is not valid. Only letters, numbers, spaces, dots and apostrophes allowed"),
        Remote(action: nameof(BooksController.IsBookUnique), controller: "Books", AdditionalFields = "Author", ErrorMessage = "This book is already present")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The author is mandatory"), 
        RegularExpression(@"^[\w\s\.]+$", ErrorMessage = "The author is not valid. Only letters, numbers, spaces and dots allowed")]
        public string Author { get; set; }
    }
}
