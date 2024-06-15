using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace HWWeb.Pages.Roles {

    [Authorize(Roles = "Admin")]

    public class RolesModel : PageModel
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public RolesModel(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public List<IdentityRole> Roles { get; set; }

            [BindProperty]
            public string RoleName { get; set; }

            public string ErrorMessage { get; set; }

            public async Task OnGetAsync()
            {
                Roles = new List<IdentityRole>(_roleManager.Roles);
            }

            public async Task<IActionResult> OnPostAddRoleAsync()
            {
                if (ModelState.IsValid)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(RoleName);
                    if (!roleExists)
                    {
                        var result = await _roleManager.CreateAsync(new IdentityRole(RoleName));
                        if (result.Succeeded)
                        {
                            return RedirectToPage();
                        }
                        else
                        {
                            ErrorMessage = "Failed to add role.";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Role already exists.";
                    }
                }
                return Page();
            }

            public async Task<IActionResult> OnPostDeleteRoleAsync(string roleName)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToPage();
                    }
                    else
                    {
                        ErrorMessage = "Failed to delete role.";
                    }
                }
                return Page();
            }
        }
    }


