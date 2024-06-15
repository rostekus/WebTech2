using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HWWeb.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HWWeb.Pages
{
    [Authorize(Roles = "Admin, User")]

    public class DetailsModel : PageModel
    {
        private readonly Services.AppDBContext _context;

        public DetailsModel(Services.AppDBContext context)
        {
            _context = context;
        }

        public Phone Phone { get; set; } = default!;
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phone.FirstOrDefaultAsync(m => m.Id == id);
            if (phone == null)
            {
                return NotFound();
            }
            else
            {
                Phone = phone;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int phoneId, int quantity)
        {
            var phone = await _context.Phone.FindAsync(phoneId);
            if (phone == null)
            {
                return NotFound();
            }

            if (quantity > phone.Inventory)
            {
                ErrorMessage = "Quantity exceeds available inventory.";
                Phone = phone; // Re-populate the Phone property to re-render the page with error
                return Page();
            }

            var userId = User.Identity.Name; // Assuming user authentication is handled
            var cart = await _context.Cart
                                    .Include(c => c.Items)
                                    .ThenInclude(ci => ci.Phone)
                                    .FirstOrDefaultAsync(c => c.UserId == userId)
                                    ?? new Models.Cart { UserId = userId, Items = new List<CartItem>() };

            var cartItem = cart.Items.FirstOrDefault(ci => ci.PhoneId == phoneId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    PhoneId = phoneId,
                    Quantity = quantity,
                    Phone = phone
                });
            }

            phone.Inventory -= quantity; // Update the inventory

            if (cart.Id == 0) // New cart
            {
                _context.Cart.Add(cart);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Cart/Index"); // Redirect to the cart page
        }
    }
}
