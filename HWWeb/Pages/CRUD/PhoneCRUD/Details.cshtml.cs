using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HWWeb.Models;
using HWWeb.Services;

namespace HWWeb.Pages.CRUD.PhoneCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly HWWeb.Services.AppDBContext _context;

        public DetailsModel(HWWeb.Services.AppDBContext context)
        {
            _context = context;
        }

        public Phone Phone { get; set; } = default!;

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
    }
}
