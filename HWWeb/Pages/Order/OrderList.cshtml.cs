using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HWWeb.Models;
using HWWeb.Services;
using Microsoft.AspNetCore.Identity;

namespace HWWeb.Pages.Order
{
    [Authorize(Roles = "Admin, User")] 
    public class OrderListModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;


        public OrderListModel(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Models.Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
          //  var userId = _userManager.GetUserId(User);
            Orders = await _context.Order
                                    .Where(o => o.UserId == User.Identity.Name)
                                    .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Phone)
                                    .ToListAsync();
            foreach (var o in Orders) {
                o.User = await _userManager.FindByNameAsync(User.Identity.Name);
                 
            }
            return Page();
        }
    }
}
