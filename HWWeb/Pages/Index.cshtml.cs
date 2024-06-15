using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HWWeb.Models;
using HWWeb.Services;
using Microsoft.AspNetCore.Authorization;

namespace HWWeb.Pages.CRUD.PhoneCRUD
{
    [Authorize(Roles = "Admin, User")]
    public class IndexPhondModel : PageModel
    {
        private readonly HWWeb.Services.AppDBContext _context;

        public IndexPhondModel(HWWeb.Services.AppDBContext context)
        {
            _context = context;
        }

        public IList<Phone> Phone { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MaxPrice { get; set; }

        public async Task OnGetAsync()
        {
            var phones = from p in _context.Phone
                         select p;

            if (!string.IsNullOrEmpty(SearchString))
            {
                phones = phones.Where(s => s.Model.Contains(SearchString));
            }

            if (MinPrice.HasValue)
            {
                phones = phones.Where(s => s.Price >= MinPrice.Value);
            }

            if (MaxPrice.HasValue)
            {
                phones = phones.Where(s => s.Price <= MaxPrice.Value);
            }

            Phone = await phones.ToListAsync();
        }
    }
}
