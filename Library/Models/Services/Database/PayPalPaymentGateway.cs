using Library.Models.Enums;
using Library.Models.Exceptions;
using Library.Models.InputModels;
using Library.Models.Options;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Services.Database
{
    public class PayPalPaymentGateway : IPaymentGateway
    {
        private readonly IOptionsMonitor<PayPalOptions> options;

        public PayPalPaymentGateway(IOptionsMonitor<PayPalOptions> options)
        {
            this.options = options;
        }

        public async Task<string> GetPaymentUrlAsync(BookPayInputModel inputModel)
        {
            OrderRequest order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = inputModel.ReturnUrl,
                    CancelUrl = inputModel.CancelUrl,
                    BrandName = options.CurrentValue.BrandName,
                    ShippingPreference = "NO_SHIPPING"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        CustomId = $"{inputModel.BookId}/{inputModel.UserId}",
                        Description = inputModel.Description,
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = inputModel.Price.Currency.ToString(),
                            Value = inputModel.Price.Amount.ToString(CultureInfo.InvariantCulture)
                        }
                    }
                }
            };
            PayPalEnvironment env = GetPayPalEnvironment(options.CurrentValue);
            PayPalHttpClient client = new PayPalHttpClient(env);
            OrdersCreateRequest request = new OrdersCreateRequest();
            request.RequestBody(order);
            request.Prefer("return=representation");
            HttpResponse response = await client.Execute(request);
            Order result = response.Result<Order>();
            LinkDescription link = result.Links.Single(link => link.Rel == "approve");
            return link.Href;
        }

        public async Task<BookPurchaseInputModel> CapturePaymentAsync(string token)
        {
            PayPalEnvironment env = GetPayPalEnvironment(options.CurrentValue);
            PayPalHttpClient client = new PayPalHttpClient(env);
            OrdersCaptureRequest request = new OrdersCaptureRequest(token);
            request.RequestBody(new OrderActionRequest());
            request.Prefer("return=representation");

            try
            {
                HttpResponse response = await client.Execute(request);
                Order result = response.Result<Order>();
                PurchaseUnit purchaseUnit = result.PurchaseUnits.First();
                Capture capture = purchaseUnit.Payments.Captures.First();
                string[] customIdParts = purchaseUnit.CustomId.Split('/');
                int bookId = int.Parse(customIdParts[0]);
                string userId = customIdParts[1];
                return new BookPurchaseInputModel
                {
                    BookId = bookId,
                    UserId = userId,
                    Paid = new ValueObjects.Money(Enum.Parse<Currency>(capture.Amount.CurrencyCode), decimal.Parse(capture.Amount.Value, CultureInfo.InvariantCulture)),
                    TransactionId = capture.Id,
                    PaymentDate = DateTime.Parse(capture.CreateTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                };
            }
            catch (Exception exc)
            {
                throw new PaymentGatewayException(exc);
            }
        }

        private PayPalEnvironment GetPayPalEnvironment(PayPalOptions options)
        {
            string clientId = options.ClientId;
            string clientSecret = options.ClientSecret;
            return options.IsSandbox ? new SandboxEnvironment(clientId, clientSecret) : new LiveEnvironment(clientId, clientSecret);
        }
    }
}
