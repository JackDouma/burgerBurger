using burgerBurger.Data;
using burgerBurger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using burgerBurger.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Stripe;
using Stripe.Checkout;
using Stripe.Terminal;

namespace burgerBurger.Controllers
{
    public class ShopController : Controller
    {
        // manually add db connection dependency (auto-generated in scaffolded controllers but this is custom)
        private readonly ApplicationDbContext _context;

        // add Configuration dependency so we can read the Stripe API key from appsettings or the Azure Config section
        private readonly IConfiguration _configuration;

        public ShopController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewData["types"] = new List<StaticItemType> { StaticItemType.Premade, StaticItemType.Side, StaticItemType.Drink };
            return View();
        }

        public IActionResult Category(StaticItemType itemType)
        {
            if (itemType != null)
            {
                // pass input param val to ViewData for display in the view
                ViewData["itemType"] = itemType;

                // fetch products for display
                var products = _context.StaticItem
                    .Where(p => p.Type == itemType)
                    .Where(i => i.IsSelling)
                    .OrderBy(p => p.Name)
                    .ToList();

                return View(products);
            }

            return RedirectToAction("Index");
        }

        // GET: /products/AddToCart/
        public IActionResult AddToCart(int ItemId, int Quantity)
        {
            var product = _context.OrderItem.Find(ItemId);

            // check if this cart already contains this product
            var cartItem = _context.CartItems.SingleOrDefault(c => c.ItemId == ItemId && c.CustomerId == GetCustomerId());

            if (cartItem == null)
            {
                // create new CartItem and populate the fields
                cartItem = new CartItem
                {
                    ItemId = ItemId,
                    Quantity = Quantity,
                    Price = (decimal)product.Price,
                    CustomerId = GetCustomerId()
                };

                _context.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += Quantity;
                _context.Update(cartItem);
            }

            _context.SaveChanges();

            return RedirectToAction("Cart");
        }

        public IActionResult RemoveFromCart(int id)
        {
            _context.CartItems.Remove(_context.CartItems.Find(id));
            _context.SaveChanges();
            return RedirectToAction("Cart");
        }

        private string GetCustomerId()
        {
            // check if we already have a session var for CustomerId
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                // create new session var of type string using a Guid
                HttpContext.Session.SetString("CustomerId", Guid.NewGuid().ToString());
            }

            return HttpContext.Session.GetString("CustomerId");
        }

        // GET: /Shop/Cart => display current user's shopping cart
        public IActionResult Cart()
        {
            // identify which cart to display
            var customerId = GetCustomerId();

            // join to parent object so we can also show the Product details
            var cartItems = _context.CartItems
                .Include(c => c.Item)
                .Where(c => c.CustomerId == customerId)
                .ToList();

            // calc cart total for display
            var total = (from c in cartItems
                         select c.Quantity * c.Item.Price).Sum();
            ViewData["Total"] = total;

            // calc and store cart quantity total in a session var for display in navbar
            var itemCount = (from c in cartItems
                             select c.Quantity).Sum();
            HttpContext.Session.SetInt32("ItemCount", itemCount);

            return View(cartItems);
        }

        // GET: /Shop/Checkout => show empty checkout page to capture customer info
        [Authorize]
        public IActionResult Checkout()
        {
            var now = TimeOnly.FromDateTime(DateTime.Now);
            var locations = _context.Location.AsEnumerable().Where(l => l.OpeningTime.Value.Ticks < now.Ticks).Where(l => l.ClosingTime.Value.Ticks > now.Ticks);
            ViewData["Locations"] = new SelectList(locations, "LocationId", "DisplayName");
            ViewData["balance"] = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.balance;
            return View();
        }

        // POST: /Shop/Checkout => create Order object and store as session var before payment
        [HttpPost]
        [Authorize]
        public IActionResult Checkout([Bind("FirstName,LastName,Address,City,Province,PostalCode,Phone,LocationId,DeliveryDate,UsedBalance")] Order order)
        {
            // 7 fields bound from form inputs in method header
            // now auto-fill 3 of the fields we removed from the form
            if (order.DeliveryDate == DateTime.MinValue)
            {
                order.DeliveryDate = DateTime.Now;
                order.Status = "Pending";
            }
            else
            {
                order.Status = "To Be Delivered";
            }

            var location = _context.Location.FindAsync(order.LocationId).Result.ClosingTime.Value.Ticks;
            if (TimeOnly.FromDateTime(DateTime.Now).Ticks > location)
            {
                var now = TimeOnly.FromDateTime(DateTime.Now);
                var locations = _context.Location.AsEnumerable().Where(l => l.OpeningTime.Value.Ticks < now.Ticks).Where(l => l.ClosingTime.Value.Ticks > now.Ticks);
                ViewData["Locations"] = new SelectList(locations, "LocationId", "DisplayName");
                ViewData["error"] = "This location is currently closed. Please select another.";
                return View(order);
            }
            ViewData["error"] = null;

            order.OrderDate = DateTime.Now;
            order.CustomerId = User.Identity.Name;

            order.OrderTotal = (decimal)(from c in _context.CartItems
                                where c.CustomerId == HttpContext.Session.GetString("CustomerId")
                                select c.Quantity * c.Item.Price).Sum();

            // store the order as session var so we can proceed to payment attempt
            HttpContext.Session.SetObject("Order", order);

            // redirect to payment
            var user = _context.Users.FirstOrDefault(u => u.UserName == order.CustomerId);
            if (user.balance > order.OrderTotal)
            {
                return RedirectToAction("UseBalance");
            }
            return RedirectToAction("Payment");
        }

        [Authorize]
        public IActionResult UseBalance()
        {
            ViewData["balance"] = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).balance;
            ViewData["total"] = HttpContext.Session.GetObject<Order>("Order").OrderTotal;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UseBalance(string result)
        {
            if (result.Equals("true"))
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var total = HttpContext.Session.GetObject<Order>("Order").OrderTotal;
                user.balance -= total;
                _context.Update(user);
                _context.SaveChanges();
                var balanceChange = new BalanceAddition { Amount = 0 - total, Balance = user.balance, CustomerId = User.Identity.Name, PaymentDate = DateTime.Now };
                _context.BalanceAdditions.Add(balanceChange);
                _context.SaveChanges();
                return RedirectToAction("SaveOrder");
            }
            return RedirectToAction("Payment");
        }

        // GET: /Shop/Payment => invoke Stripe payment session which displays their payment form
        [Authorize]
        public IActionResult Payment()
        {
            // get the order from the session var
            var order = HttpContext.Session.GetObject<Order>("Order");

            Session session = PaymentMethods.Payment(_configuration, order.OrderTotal, Request.Host.Value, "/Shop/SaveOrder", "/Shop/Cart");

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        // GET: /Shop/SaveOrder => create Order in DB, add OrderDetails, clear cart
        [Authorize]
        public IActionResult SaveOrder()
        {        
            // get the order from session var
            var order = HttpContext.Session.GetObject<Order>("Order");

            // save each CartItem as a new OrderDetails record for this order
            var cartItems = _context.CartItems.Where(c => c.CustomerId == HttpContext.Session.GetString("CustomerId"));


            // get inventory
            var inventoryItem = _context.Inventory
              .Where(i => i.Location.LocationId == order.LocationId)
              .Where(i => i.itemThrowOutCheck == false)
              .OrderBy(i => i.itemExpirey)
              .ToList();

            bool inventoryCheck = false;
            // loop through each cartitem
            foreach (var cart in cartItems)
            {
                foreach (var ingredient in cart.Item.Ingredients)
                {
                    inventoryCheck = false;
                    // loop through each inventory item
                    foreach (var inventory in inventoryItem)
                    {
                        // only loop if inventory has not been found
                        if (inventoryCheck == false)
                        {
                            // if ingredients match
                            if (inventory.itemName == ingredient.itemName)
                            {
                                // if there is a enough ingredients available at store
                                if (inventory.quantity >= (1 * cart.Quantity))
                                {
                                    inventory.quantity = inventory.quantity - (1 * cart.Quantity);
                                    inventoryCheck = true;
                                }
                            }
                        }
                    }

                    // if false, ingredient was not found; cancel transaction
                    if (inventoryCheck == false)
                    {
                        return RedirectToAction("Cart", "Shop", new { result = "outOfStock" });
                    }
                }             
            }





            // fill required PaymentCode temporarily
            order.PaymentCode = HttpContext.Session.GetString("CustomerId");

            // save new order to db
            _context.Add(order);
            _context.SaveChanges();   

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Quantity,
                    ItemId = item.ItemId,
                    Price = item.Price,
                    OrderId = order.OrderId
                };
                _context.Add(orderDetail);
            }

            _context.SaveChanges();

            // empty cart
            foreach (var item in cartItems)
            {
                _context.Remove(item);
            }
            _context.SaveChanges();

            // clear session variables
            HttpContext.Session.Clear();

            // redirect to Order Confirmation
            return RedirectToAction("Details", "Orders", new { @id = order.OrderId });
        }
    }
}
