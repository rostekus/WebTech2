using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HWWeb.Models;

namespace HWWeb.Services
{
    public class AppDBContext: IdentityDbContext<User>
    {
public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {

        }

        public DbSet<HWWeb.Models.Phone> Phone { get; set; } = default!;


    }
}
