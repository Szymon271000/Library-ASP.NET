using Library.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            switch (feature.Error)
            {
                case BookNotFoundException exc:
                    {
                        ViewData["Title"] = "Book not found";
                        Response.StatusCode = 404;
                        return View("BookNotFound");
                    }

                case BookPurchaseException exc:
                    {
                        ViewData["Title"] = "Purchase unsuccessful";
                        Response.StatusCode = 400;
                        return View();
                    }

                case PaymentGatewayException exc:
                    {
                        ViewData["Title"] = "An error occured during payment";
                        Response.StatusCode = 400;
                        return View();
                    }

                default:
                    {
                        ViewData["Title"] = "Error";
                        return View();
                    }
            }
        }
    }
}
