﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HWWeb.Models;
using HWWeb.Services;

namespace HWWeb.Pages.CRUD.CartItemCRUD
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
            return Page();
        }

        [BindProperty]
        public CartItem CartItem { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CartItem.Add(CartItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
