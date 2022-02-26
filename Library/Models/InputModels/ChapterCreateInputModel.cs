using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    public class ChapterCreateInputModel
    {
        [Required]
        public int BookId { get; set; }

        [Required(ErrorMessage = "The title is mandatory"),
        MinLength(5, ErrorMessage = "The title should contain a minimum of {1} characters"),
        MaxLength(100, ErrorMessage = "The title should contain a maximum of {1} characters")]
        public string Title { get; set; }
    }
}
