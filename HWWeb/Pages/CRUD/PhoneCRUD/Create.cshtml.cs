using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HWWeb.Models;
using HWWeb.Services;
using Microsoft.AspNetCore.Authorization;

namespace HWWeb.Pages.CRUD.PhoneCRUD
{
    [Authorize(Roles = "Admin")]

    public class CreateModel : PageModel
    {
        private readonly HWWeb.Services.AppDBContext _context;

        public CreateModel(HWWeb.Services.AppDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Phone Phone { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Phone.Inventory < 0)
            {
                ModelState.AddModelError("Phone.Inventory", "Inventory must be greater than or equal to zero.");
                return Page();
            }

            if (Phone.Price <= 0)
            {
                ModelState.AddModelError("Phone.Price", "Price must be greater than zero.");
                return Page();
            }

            if (!IsValidPrice(Phone.Price))
            {
                ModelState.AddModelError("Phone.Price", "Price must have exactly two decimal places.");
                return Page();
            }

            _context.Phone.Add(Phone);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool IsValidPrice(decimal price)
        {
            var priceString = price.ToString();
            var dotIndex = priceString.IndexOf('.');
            if (dotIndex == -1)
                return false;

            return priceString.Substring(dotIndex + 1).Length == 2;
        }
    }
}
