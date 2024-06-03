using Microsoft.AspNetCore.Identity;

namespace HWWeb.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";

        public string Address { get; set; } = "";
        public DateTime CreatAt { get; set; } = DateTime.Now;
    }
}
