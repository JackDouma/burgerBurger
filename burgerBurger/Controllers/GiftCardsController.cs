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

namespace burgerBurger.Controllers
{
    public class GiftCardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GiftCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GiftCards
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

                        _context.Users.Update(user);
                        _context.GiftCards.Update(giftCard);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", new { result = "success" });
                    }
                    // if user is not logged in
                    else
                    {
                        return RedirectToAction("Index", new { result = "invalidUser" });
                    }
                }
                // invalid code
                else
                {
                    return RedirectToAction("Index", new { result = "invalidCode" });
                }

            }
            return RedirectToAction("Index", new { result = "error" });
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
        public async Task<IActionResult> Create([Bind("GiftCardId,amount,code,redeemed")] GiftCard giftCard)
        {
            // IF STATEMENT HERE FOR PAYMENT

            if (ModelState.IsValid)
            {             

                // generate code
                string generateCode = GenerateCode(16);

                // CODE HERE TO SEND EMAIL OR TEXT MESSAGE OF GIFTCARD CODE

                // hash code
                string hashCode = HashCode(generateCode);

                // assign var
                giftCard.code = hashCode;

                _context.Add(giftCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(giftCard);
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
