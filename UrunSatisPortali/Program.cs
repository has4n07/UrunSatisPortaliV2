using Microsoft.EntityFrameworkCore;
using UrunSatisPortali.Data;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity;
using UrunSatisPortali.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
try
{
    var hostName = "oauth2.googleapis.com";
    var ips = System.Net.Dns.GetHostAddresses(hostName);
    Console.WriteLine($"[DIAGNOSTIC] DNS Lookup for {hostName} Succeeded:");
    foreach (var ip in ips)
    {
        Console.WriteLine($"[DIAGNOSTIC] found IP: {ip}");
    }

    using (var client = new HttpClient())
    {
        client.Timeout = TimeSpan.FromSeconds(5);
        Console.WriteLine("[DIAGNOSTIC] Attempting HTTP connection to https://oauth2.googleapis.com...");
        var response = await client.GetAsync("https://oauth2.googleapis.com");
        Console.WriteLine($"[DIAGNOSTIC] HTTP Connection Succeeded. Status: {response.StatusCode}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[DIAGNOSTIC] Diagnostic Check FAILED: {ex.Message}");
    if (ex.InnerException != null) Console.WriteLine($"[DIAGNOSTIC] Inner: {ex.InnerException.Message}");
}

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // Diagnostic logging
    var configDebug = ((IConfigurationRoot)builder.Configuration).GetDebugView();
    Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
    Console.WriteLine("Configuration Debug View:");
    Console.WriteLine(configDebug);

    throw new InvalidOperationException("Connection string 'DefaultConnection' not found. See console output for configuration details.");
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Access/Login";
    options.LogoutPath = "/Access/Logout";
    options.AccessDeniedPath = "/Access/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
});

// Add Google Authentication
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

// Add Notyf
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<OrderHub>("/orderHub");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    var adminUser = await userManager.FindByEmailAsync("admin@test.com");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = "admin@test.com",
            Email = "admin@test.com",
            EmailConfirmed = true,
            NameSurname = "Admin User",
            Role = "Admin"
        };
        var createResult = await userManager.CreateAsync(adminUser, "Admin123!");
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

app.Run();
