using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using burgerBurger.Data;
using burgerBurger.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using Stripe.Checkout;
using burgerBurger.Data.Migrations;
using Stripe;

namespace burgerBurger.Controllers
{
    [Authorize(Roles = "Admin,Customer")]
    public class GiftCardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration; 

        public GiftCardsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: GiftCards
        [Authorize (Roles = "Admin,Owner")]
        public async Task<IActionResult> Index()
        {
              return _context.GiftCards != null ? 
                          View(await _context.GiftCards.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.GiftCards'  is null.");
        }

        //GET: Inventories/redeem
        public IActionResult Redeem()
        {
            return View();
        }

        // POST: GiftCards/Redeem
        [HttpPost]
        public async Task<IActionResult> Redeem(string enteredCode)
        {
            if (ModelState.IsValid)
            {
                // hash code
                string hashEnteredCode = HashCode(enteredCode);

                // compare entered code to all active codes
                var giftCard = await _context.GiftCards.FirstOrDefaultAsync(gc => gc.code == hashEnteredCode);

                // valid code
                if (giftCard != null && giftCard.redeemed == false)
                {
                    // get logged in user
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                    // if user is logged in
                    if (user != null)
                    {
                        user.balance += giftCard.amount;
                        giftCard.redeemed = true;
                        var balanceChange = new BalanceAddition { Amount = giftCard.amount, Balance = user.balance, CustomerId = User.Identity.Name, PaymentDate = DateTime.Now };
                        _context.Users.Update(user);
                        _context.GiftCards.Update(giftCard);
                        _context.BalanceAdditions.Add(balanceChange);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "BalanceAdditions", new { result = "success" });
                    }
                    // if user is not logged in
                    else
                    {
                        return RedirectToAction("Index", "Index", new { result = "invalidUser" });
                    }
                }
                // invalid code
                else
                {
                    return RedirectToAction("Index", "Home", new { result = "invalidCode" });
                }

            }
            return RedirectToAction("Index", "Home", new { result = "error" });
        }

        // GET: GiftCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GiftCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GiftCardId,amount,code,redeemed, giftPhoneNumber")] GiftCard giftCard) //string giftPhoneNum
        {
            // IF STATEMENT HERE FOR PAYMENT

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetObject("GiftCard", giftCard);
                return RedirectToAction("Payment");

            }
            return View(giftCard);
        }

        [Authorize(Roles = "Customer,Admin")]
        public IActionResult Payment()
        {
            // get the order from the session var
            var giftCard = HttpContext.Session.GetObject<GiftCard>("GiftCard");

            Session session = PaymentMethods.Payment(_configuration, giftCard.amount, Request.Host.Value, "/GiftCards/SaveOrder", "/GiftCards/Create");

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [Authorize(Roles = "Customer,Admin")]
        public IActionResult SaveOrder()
        {

            var giftCard = HttpContext.Session.GetObject<GiftCard>("GiftCard");

            // generate code
            string generateCode = GenerateCode(16);

            // hash code
            string hashCode = HashCode(generateCode);

            // assign var
            giftCard.code = hashCode;

            // assign user who bought it
            giftCard.CustomerId = User.Identity.Name;

            // TRIAL ACCOUNT OF TWILIO WE CAN ONLY SEND TO VERIFIED NUMBERS (ALESSIO, MIKEY, JACK CURRENTLY)
            TwilioClient.Init(_configuration.GetValue<string>("Twilio:AccountSID"), _configuration.GetValue<string>("Twilio:AuthToken"));
            var to = new PhoneNumber(giftCard.giftPhoneNumber);
            var from = new PhoneNumber(_configuration.GetValue<string>("Twilio:PhoneNumber"));
            var message = MessageResource.Create(
                to: to,
            from: from,
                body: $"You have recieved a ${giftCard.amount}.00 gift card for BurgerBurger! Enter {generateCode} on our website to redeem!");

            //return Content(message.Sid);

            _context.Add(giftCard);
            _context.SaveChangesAsync();

            // clear session variables
            HttpContext.Session.Clear();

            return RedirectToAction("Create", new { result = "success" });
        }

        private bool GiftCardExists(int id)
        {
             return (_context.GiftCards?.Any(e => e.GiftCardId == id)).GetValueOrDefault();
        }

        /**
         * This method will create a random code and return it
         */
        private string GenerateCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";

            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /**
         * This method will hash a code that is given and return
         */
        private string HashCode(string code)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // convert string to a byte array and hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(code));

                // convert the byte array to string
                StringBuilder builder = new StringBuilder();

                for (int x = 0; x < bytes.Length; x++)
                {
                    builder.Append(bytes[x].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
