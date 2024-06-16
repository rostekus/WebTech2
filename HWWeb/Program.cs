using HWWeb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HWWeb.Models;
using HWWeb.Controllers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


// Add services to the container.
builder.Services.AddRazorPages().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

builder.Services.AddControllers();
builder.Services.AddScoped<RolesController>();

builder.Services.AddDbContext<AppDBContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connStr);

});

builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddLogging(builder =>
{
    builder.AddConsole(); // Write logs to the console
});
   

builder.Services.AddIdentity<User, IdentityRole>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDBContext>();

var app = builder.Build();


var supportedCultures = new[] { "en", "pl" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    db.Database.Migrate();
}


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new string[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    string email = "admin123@admin.com";
    string password = "Admin123!";

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new HWWeb.Models.User();
        user.Email = email;
        user.UserName = email;
        user.EmailConfirmed = true;
        userManager.CreateAsync(user, password).Wait();
        var res = await userManager.AddToRoleAsync(user, "Admin");

        if (!res.Succeeded)
        {
            foreach (var error in res.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
        

    }
    app.Run();
}

