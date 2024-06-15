using HWWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HWWeb.Pages.Order
{



    [Authorize(Roles = "Admin, User")]
    public class DetailsModel : PageModel
    {
        private readonly AppDBContext _context;

        public DetailsModel(AppDBContext context)
        {
            _context = context;
        }

        public Models.Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Order.FindAsync(id);
            var items = _context.OrderItem.Where(oi => oi.OrderId == id).ToList<Models.OrderItem>();
            
            Order.OrderItems = items;
            foreach (var item in Order.OrderItems)
            {
                item.Phone = _context.Phone.Find(item.PhoneId);
            }
            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
