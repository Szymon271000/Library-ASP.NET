using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Options
{
    public partial class BooksOptions
    {
        public int PerPage { get; set; }
        public int InHome { get; set; }

        public BooksOrderOptions Order { get; set; }
    }

    public partial class BooksOrderOptions
    {
        public string By { get; set; }

        public bool Ascending { get; set; }

        public string[] Allow { get; set; }
    }
}
