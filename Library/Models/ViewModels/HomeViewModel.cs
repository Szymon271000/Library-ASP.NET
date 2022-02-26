using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<BookViewModel> MostRecentBooks { get; set; }
        public List<BookViewModel> BestRatingBooks { get; set; }
    }
}
