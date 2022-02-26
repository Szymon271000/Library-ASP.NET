using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Services.Database
{
    public interface IBookCoverPersister
    {
        Task<string> SaveBookCoverAsync(int bookId, IFormFile formFile);
    }
}
