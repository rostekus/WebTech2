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
        public DbSet<HWWeb.Models.CartItem> CartItem { get; set; } = default!;
        public DbSet<HWWeb.Models.Cart> Cart { get; set; } = default!;
        public DbSet<HWWeb.Models.Order> Order { get; set; } = default!;
        public DbSet<HWWeb.Models.OrderItem> OrderItem { get; set; } = default!;

    }
}
