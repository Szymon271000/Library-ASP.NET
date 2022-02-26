using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    public class ChapterDeleteInputModel
    {
        [Required]
        public int Id { get; set; }
        public int BookId { get; set; }
    }
}
