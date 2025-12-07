using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrunSatisPortali.Models;
using UrunSatisPortali.Repository;
using UrunSatisPortali.Data;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Security.Cryptography;
using System.Text;

using UrunSatisPortali.ViewModels;

namespace UrunSatisPortali.Controllers
{
    public class AccessController(ApplicationDbContext context, INotyfService notyf, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly INotyfService _notyf = notyf;
        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == model.Email);

                if (user != null && VerifyPassword(model.Password, user.PasswordHash))
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.AdSoyad ?? user.Email ?? ""),
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(ClaimTypes.Role, user.Role),
                        new("ProfilePhotoPath", user.ProfilePhotoPath ?? "/images/default-user.png")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    _notyf.Success("Giriş Başarılı");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _notyf.Error("Kullanıcı bulunamadı veya şifre hatalı");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = _context.Users.Any(x => x.Email == model.Email);
                if (userExists)
                {
                    _notyf.Error("Bu email adresi ile kayıtlı kullanıcı mevcut.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    AdSoyad = model.AdSoyad,
                    EmailConfirmed = true, // Auto confirm for manual reg
                    Role = "User"
                };

                // Manual Password Hashing
                user.PasswordHash = HashPassword(model.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _notyf.Success("Kayıt Başarılı. Giriş yapabilirsiniz.");
                return RedirectToAction("Login");
            }
            return View(model);
        }

        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string password, string? hash)
        {
            if (string.IsNullOrEmpty(hash)) return false;
            return HashPassword(password) == hash;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _notyf.Success("Çıkış Yapıldı");
            return RedirectToAction("Login", "Access");
        }

        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // If cookie auth failed, try to authenticate with the external cookie (Google sets this temporarily)
            // Actually, in this flow, Google middleware should have handled the callback and signed in the user 
            // to the scheme we asked for in Challenge? 
            // Wait, usually we use an external cookie scheme for the intermediate step.
            // But here we are using the same CookieAuthenticationDefaults.AuthenticationScheme?
            // Let's check how AddGoogle works with AddCookie.
            // Usually: .AddCookie() .AddGoogle()
            // When Challenge(Google), it redirects to Google.
            // On return, Google middleware validates and signs in the user. 
            // BUT, if we want to do custom logic (like checking DB), we might need to inspect the principal.

            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            // Wait, Google handler usually signs in to the default scheme or specific scheme.
            // Let's use a simpler approach: 
            // We want to capture the user info returned by Google.

            // Correct pattern often involves:
            // 1. Challenge Google
            // 2. Google redirects back to /signin-google (handled by middleware)
            // 3. Middleware constructs principal and calls SignInAsync on the configured scheme (Cookies).

            // HOWEVER, we want to check our DB.
            // So we should probably use a separate scheme for external auth, OR hook into the events.
            // OR, simpler: Let Google sign in, then in this Action (which is NOT the callback path, wait).
            // The callback path is usually /signin-google.

            // Ah, I set RedirectUri = Url.Action("GoogleResponse").
            // So Google will redirect HERE after the middleware processes the return?
            // No, the middleware handles the callback path.
            // If we set RedirectUri to this action, the middleware will redirect HERE after it's done.
            // So at this point, the user SHOULD be authenticated via the Cookie scheme if the middleware did its job.

            // Let's verify.
            var result2 = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result2.Succeeded)
            {
                // User is already signed in by Google middleware? 
                // No, Google middleware creates a principal. It doesn't necessarily sign in to the app cookie unless configured.
                // But we chained .AddGoogle() to .AddAuthentication().

                // Let's look at the claims.
                var claims = result2.Principal.Identities.FirstOrDefault()?.Claims;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                            ?? claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value; // Google often sends email as Name

                if (email != null)
                {
                    var user = _context.Users.FirstOrDefault(x => x.Email == email);
                    if (user == null)
                    {
                        // Auto-register
                        user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            AdSoyad = result2.Principal.FindFirst(ClaimTypes.Name)?.Value ?? email,
                            EmailConfirmed = true,
                            Role = "User"
                        };
                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();
                    }

                    // Now we ensure the user is logged in with OUR claims (Role, Id etc)
                    var appClaims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.Email ?? ""),
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(appClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    _notyf.Success($"Google ile giriş yapıldı: {user.Email}");
                    return RedirectToAction("Index", "Home");
                }
            }

            _notyf.Error("Google girişi başarısız oldu.");
            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            var model = new ProfileViewModel
            {
                AdSoyad = user.AdSoyad ?? "",
                Email = user.Email ?? "",
                CurrentProfilePhotoPath = user.ProfilePhotoPath
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            user.AdSoyad = model.AdSoyad;
            // Email update logic can be complex (re-verification), so maybe skip or handle carefully. 
            // For now, let's allow updating AdSoyad and Photo.

            if (model.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePhoto.CopyToAsync(fileStream);
                }
                user.ProfilePhotoPath = "/images/profiles/" + uniqueFileName;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Refresh Cookie to update Claims (Name, Photo)
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.AdSoyad ?? user.Email ?? ""),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Role, user.Role),
                new("ProfilePhotoPath", user.ProfilePhotoPath ?? "/images/default-user.png")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            _notyf.Success("Profil güncellendi.");
            return RedirectToAction("Profile");
        }
    }
}
