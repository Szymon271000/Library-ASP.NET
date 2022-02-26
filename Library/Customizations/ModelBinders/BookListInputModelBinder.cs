using Library.Models.InputModels;
using Library.Models.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Customizations.ModelBinders
{
    public class BookListInputModelBinder : IModelBinder
    {
        private readonly IOptionsMonitor<BooksOptions> booksOptions;
        public BookListInputModelBinder(IOptionsMonitor<BooksOptions> booksOptions)
        {
            this.booksOptions = booksOptions;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string search = bindingContext.ValueProvider.GetValue("Search").FirstValue;
            string orderby = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue;
            int.TryParse(bindingContext.ValueProvider.GetValue("Page").FirstValue, out int page);
            bool.TryParse(bindingContext.ValueProvider.GetValue("Ascending").FirstValue, out bool ascending);
            BooksOptions options = booksOptions.CurrentValue;
            var inputModel = new BookListInputModel(search, page, orderby, ascending, options.PerPage, options.Order);
            bindingContext.Result = ModelBindingResult.Success(inputModel);
            return Task.CompletedTask;
        }
    }
}
