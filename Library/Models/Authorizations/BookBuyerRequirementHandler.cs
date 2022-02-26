using Library.Models.Services.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Models.Authorizations
{
    public class BookBuyerRequirementHandler : AuthorizationHandler<BookBuyerRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBookService bookService;

        public BookBuyerRequirementHandler(IHttpContextAccessor httpContextAccessor, IBookService bookService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.bookService = bookService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BookBuyerRequirement requirement)
        {
            string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int bookId;
            if (context.Resource is int)
            {
                bookId = (int)context.Resource;
            }
            else
            {
                int id = Convert.ToInt32(httpContextAccessor.HttpContext.Request.RouteValues["id"]);
                if (id == 0)
                {
                    context.Fail();
                    return;
                }
                switch (httpContextAccessor.HttpContext.Request.RouteValues["controller"].ToString().ToLowerInvariant())
                {
                    case "books":
                        bookId = id;
                        break;

                    default:
                        context.Fail();
                        return;
                }
            }
            bool isPurchased = await bookService.IsBookPurchasedAsync(bookId, userId);
            if (isPurchased)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
