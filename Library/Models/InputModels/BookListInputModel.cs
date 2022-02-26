using Library.Customizations.ModelBinders;
using Library.Models.Options;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.InputModels
{
    [ModelBinder(BinderType = typeof(BookListInputModelBinder))]
    public class BookListInputModel
    {
        public BookListInputModel(string search, int page, string orderby, bool ascending, int limit, BooksOrderOptions orderOptions)
        {
            if (!orderOptions.Allow.Contains(orderby))
            {
                orderby = orderOptions.By;
                ascending = orderOptions.Ascending;
            }

            Search = search ?? "";
            Page = Math.Max(1, page);
            OrderBy = orderby;
            Ascending = ascending;
            Limit = Math.Max(1, limit);
            Offset = (Page - 1) * Limit;
        }
        public string Search { get; }
        public int Page { get; }
        public string OrderBy { get; }
        public bool Ascending { get; }
        public int Limit { get; }
        public int Offset { get; }
    }
}
