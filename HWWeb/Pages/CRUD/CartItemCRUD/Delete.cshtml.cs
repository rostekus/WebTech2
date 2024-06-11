using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HWWeb.Models;
using HWWeb.Services;

namespace HWWeb.Pages.CRUD.CartItemCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly HWWeb.Services.AppDBContext _context;

        public DeleteModel(HWWeb.Services.AppDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CartItem CartItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartitem = await _context.CartItem.FirstOrDefaultAsync(m => m.Id == id);

            if (cartitem == null)
            {
                return NotFound();
            }
            else
            {
                CartItem = cartitem;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartitem = await _context.CartItem.FindAsync(id);
            if (cartitem != null)
            {
                CartItem = cartitem;
                _context.CartItem.Remove(CartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
