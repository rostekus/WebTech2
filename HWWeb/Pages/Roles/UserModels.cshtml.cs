using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HWWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HWWeb.Pages.Roles
{
    [Authorize(Roles = "Admin")]

    public class UserModels : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserModels(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<User> Users { get; set; }
        public Dictionary<string, List<string>> UserRoles { get; set; }
        public List<string> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
            Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            UserRoles = new Dictionary<string, List<string>>();

            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserRoles.Add(user.Id, roles.ToList());
            }
        }

        public async Task<IActionResult> OnPostChangeRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, roleName);

            return RedirectToPage();
        }
    }
}
