using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using StripeDemo.Models;

namespace StripeDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Checkout()
    {
        return View();
    }
    
    public IActionResult Checkout2()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Pay([FromBody]PayData? data)
    {
        var domain = "https://localhost:44328";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            SuccessUrl = domain + "/Home/Success?id={CHECKOUT_SESSION_ID}",
            CancelUrl = domain
        };
        if (data.Prod1 > 0)
        {
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 2500,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Sports t-shirt"
                    }
                },
                Quantity = data.Prod1
            });
        }
        if (data.Prod2 > 0)
        {
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 5000,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Kettle bell"
                    }
                },
                Quantity = data.Prod2
            });
        }
        if (data.Prod3 > 0)
        {
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 7500,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Running shoes"
                    }
                },
                Quantity = data.Prod3
            });
        }
        
        var client = new StripeClient(apiKey: "sk_test_51S1MP2RzsDM5cDYa1Tfk2T3D38VzgySeWCgdRp9jqOcbYevHRvhasZ5OjcMJokdldmxjZPsq3QbHGgL8S25zUPV500b6T79Bc2");

        if (data?.Email != null)
        {
            var customerService = new CustomerService(client);
            var customerSearch = await customerService.SearchAsync(new CustomerSearchOptions
            {
                Query = $"email:'{data.Email}'"
            });
            if (customerSearch.Data != null && customerSearch.Data.Count > 0)
            {
                options.Customer = customerSearch.Data[0].Id;
            }
            else
            {
                var customer = await customerService.CreateAsync(new CustomerCreateOptions
                {
                    Name = data.Name,
                    Email = data.Email
                });
                options.Customer = customer.Id;
            }

            options.PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                SetupFutureUsage = "off_session"
            };
            options.SavedPaymentMethodOptions = new SessionSavedPaymentMethodOptionsOptions
            {
                PaymentMethodSave = "enabled"
            };
        }
        
        var service = new SessionService(client);
        var session = await service.CreateAsync(options);

        return Json(new { url = session.Url });
    }
    
    [HttpPost]
    public JsonResult Pay2([FromBody]PayData? data)
    {
        var domain = "https://localhost:44328";
        var options = new SessionCreateOptions
        {
            UiMode = "embedded",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            CustomerCreation = "always",
            ReturnUrl = domain + "/Home/Success?id={CHECKOUT_SESSION_ID}",
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                SetupFutureUsage = "off_session"
            },
            SavedPaymentMethodOptions = new SessionSavedPaymentMethodOptionsOptions()
            {
                PaymentMethodSave = "enabled"
            }
        };
        
        if (data.Prod1 > 0)
        {
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 2500,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Sports t-shirt"
                    }
                },
                Quantity = data.Prod1
            });
        }
        if (data.Prod2 > 0)
        {
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 5000,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Kettle bell"
                    }
                },
                Quantity = data.Prod2
            });
        }
        if (data.Prod3 > 0)
        {
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 7500,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Running shoes"
                    }
                },
                Quantity = data.Prod3
            });
        }
        
        var client = new StripeClient(apiKey: "sk_test_51S1MP2RzsDM5cDYa1Tfk2T3D38VzgySeWCgdRp9jqOcbYevHRvhasZ5OjcMJokdldmxjZPsq3QbHGgL8S25zUPV500b6T79Bc2");

        var service = new SessionService(client);
        var session = service.Create(options);

        return Json(new {clientSecret = session.ClientSecret});
    }
    
    [HttpPost]
    public JsonResult Pay3()
    {
        var domain = "https://localhost:44328";
        var options = new SessionCreateOptions
        {
            UiMode = "custom",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 2500,
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Sports t-shirt"
                        }
                    },
                    Quantity = 1
                },
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 5000,
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Kettle bell"
                        }
                    },
                    Quantity = 2
                }
            },
            Mode = "payment",
            CustomerCreation = "always",
            ReturnUrl = domain + "/Home/Success?id={CHECKOUT_SESSION_ID}"
        };
        
        var client = new StripeClient(apiKey: "sk_test_51S1MP2RzsDM5cDYa1Tfk2T3D38VzgySeWCgdRp9jqOcbYevHRvhasZ5OjcMJokdldmxjZPsq3QbHGgL8S25zUPV500b6T79Bc2");

        var service = new SessionService(client);
        var session = service.Create(options);

        return Json(new {clientSecret = session.ClientSecret});
    }
    
    public async Task<IActionResult> Success(string id)
    {
        var client = new StripeClient(apiKey: "sk_test_51S1MP2RzsDM5cDYa1Tfk2T3D38VzgySeWCgdRp9jqOcbYevHRvhasZ5OjcMJokdldmxjZPsq3QbHGgL8S25zUPV500b6T79Bc2");
        var service = new SessionService(client);
        var session = await service.GetAsync(id);
        return View(new Success { Status = session.PaymentStatus, Amount = (decimal)session.AmountTotal / 100 });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}