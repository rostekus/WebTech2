using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HWWeb.Models;
using HWWeb.Services;

namespace HWWeb.Pages.Cart
{
    public class PaymentModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public PaymentModel(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Models.Cart Cart { get; set; }

        [BindProperty]
        public string ShippingAddress { get; set; }

        [BindProperty]
        public string BankAccount { get; set; }

        public string ErrorMessage { get; set; }

        private Models.Cart GetCart(string userId)
        {
            return _context.Cart
                           .Include(c => c.Items)
                           .ThenInclude(ci => ci.Phone)
                           .FirstOrDefault(c => c.UserId == userId)
                   ?? new Models.Cart { UserId = userId, Items = new List<CartItem>() };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.Identity.Name; // Assuming user authentication is handled
            Cart = GetCart(userId);

            if (Cart == null || !Cart.Items.Any())
            {
                return RedirectToPage("/Cart/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var userId = User.Identity.Name;
            Cart = GetCart(userId);

            var order = new Models.Order
            {
                UserId = User.Identity.Name,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            // Add the order to the context and save changes to generate the OrderId
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            // Now that the order has been saved and has an Id, create the OrderItems
            var items = Cart.Items.Select(item => new OrderItem
            {
                PhoneId = item.PhoneId,
                Quantity = item.Quantity,
                UserId = User.Identity.Name,
                OrderId = order.Id // Associate each OrderItem with the generated OrderId
            }).ToList();

            // Add the OrderItems to the context
            _context.OrderItem.AddRange(items);

            // Save the changes to the context
            await _context.SaveChangesAsync();

            // Delete the cart and associated items
            _context.RemoveRange(Cart.Items);
           // _context.Remove(Cart);
            await _context.SaveChangesAsync();

            // Redirect to the confirmation page
            return RedirectToPage("/Cart/Confirmation");

        }
    }
}
