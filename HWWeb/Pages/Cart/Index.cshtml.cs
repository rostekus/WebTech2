using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HWWeb.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HWWeb.Services;

namespace HWWeb.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly AppDBContext _context;

        public IndexModel(AppDBContext context)
        {
            _context = context;
        }

        public Models.Cart Cart { get; set; }

        private Models.Cart GetCart(string userId)
        {
            return _context.Cart
                           .Include(c => c.Items)
                           .ThenInclude(ci => ci.Phone)
                           .FirstOrDefault(c => c.UserId == userId)
                   ?? new Models.Cart { UserId = userId, Items = new List<CartItem>() };
        }

        public void OnGet()
        {
            var userId = User.Identity.Name; // Assuming user authentication is handled
            Cart = GetCart(userId);
        }

        public IActionResult OnGetRemoveFromCart(int phoneId)
        {
            var userId = User.Identity.Name; // Assuming user authentication is handled
            var cart = GetCart(userId);

            var cartItem = cart.Items.FirstOrDefault(ci => ci.PhoneId == phoneId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
