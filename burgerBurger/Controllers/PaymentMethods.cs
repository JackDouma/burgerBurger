using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using burgerBurger.Data;
using burgerBurger.Models;
using Microsoft.AspNetCore.Authorization;
using Stripe.Checkout;
using Stripe;

namespace burgerBurger.Controllers
{
    public class PaymentMethods
    {
        public static Session Payment(IConfiguration _configuration, decimal amount, string host, string successUrl, string cancelUrl)
        {
            // get the api key from the site config
            StripeConfiguration.ApiKey = _configuration.GetValue<string>("StripeSecretKey");

            // stripe invocation from https://stripe.com/docs/checkout/quickstart?client=html
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                        {
                          new SessionLineItemOptions
                          {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long?)(amount * 100), // total must be in cents, not dollars and cents
                                Currency = "cad",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "burgerBurger Purchase"
                                },
                            },
                            Quantity = 1,
                          },
                        },
                Mode = "payment",
                SuccessUrl = "https://" + host + successUrl,
                CancelUrl = "https://" + host + cancelUrl
            };
            var service = new SessionService();
            return service.Create(options);

            
        }
    }
}
