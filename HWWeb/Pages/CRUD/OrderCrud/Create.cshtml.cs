using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HWWeb.Models;
using HWWeb.Services;

namespace HWWeb.Pages.CRUD.OrderCrud
{
    public class CreateModel : PageModel
    {
        private readonly HWWeb.Services.AppDBContext _context;

        public CreateModel(HWWeb.Services.AppDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["OrderStatuses"] = GetOrderStatusSelectList();

            return Page();
        }

        [BindProperty]
        public Models.Order Order { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            _context.Order.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        private List<SelectListItem> GetOrderStatusSelectList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Pending", Text = "Pending" },
                new SelectListItem { Value = "Processing", Text = "Processing" },
                new SelectListItem { Value = "Shipped", Text = "Shipped" },
                new SelectListItem { Value = "Delivered", Text = "Delivered" },
                new SelectListItem { Value = "Canceled", Text = "Canceled" }
            };
        }

    }

}
