﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HWWeb.Models;
using HWWeb.Services;

namespace HWWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HWWeb.Services.AppDBContext _context;

        public IndexModel(HWWeb.Services.AppDBContext context)
        {
            _context = context;
        }

       
    }
}