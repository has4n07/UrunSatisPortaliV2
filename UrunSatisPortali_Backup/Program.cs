// --- Gerekli Tüm Kütüphaneler ---
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using UrunSatisPortali.Data;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using UrunSatisPortali.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Veritabanı Bağlantısı ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DEBUG: Bağlantı dizesini konsola yazdır
Console.WriteLine($"[DEBUG] Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"[DEBUG] ConnectionString: '{connectionString}'");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Artık bizim yeni modelimizi (ApplicationUser) kullan
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// --- 3. Sahte E-posta ---
builder.Services.AddScoped<IEmailSender, DummyEmailSender>();

// --- 4. Repository (Depo Görevlileri) ---
builder.Services.AddScoped<IKategoriRepository, KategoriRepository>();
builder.Services.AddScoped<IUrunRepository, UrunRepository>();

// --- 5. MVC ve Razor Pages ---
builder.Services.AddControllersWithViews();
// --- VİRGÜL/NOKTA (KÜLTÜR) SORUNU ÇÖZÜMÜ ---
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("tr-TR") };

    // Varsayılan kültür olarak 'en-US' ata
    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
// --- ÇÖZÜM BİTTİ ---
builder.Services.AddRazorPages();

var app = builder.Build();

// 1. Kültür ayarları (Virgül/Nokta sorununun çözümü)
app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 2. Yönlendirmeyi başlat
app.UseRouting();

// 3. GÜVENLİK (Middleware'in kritik sırası)
// Authentication (Kimlik Doğrulama) mutlaka Routing'den hemen sonra gelmelidir.
app.UseAuthentication(); // <-- Bu, Google'a Challenge başlatır
app.UseAuthorization();

// 4. Rotalar (Sayfaları göster)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Identity sayfaları için şart

app.Run();