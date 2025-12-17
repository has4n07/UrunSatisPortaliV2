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

using Microsoft.AspNetCore.Identity;
using UrunSatisPortali.ViewModels;

namespace UrunSatisPortali.Controllers
{
    public class AccessController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, INotyfService notyf, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly INotyfService _notyf = notyf;
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
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
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
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NameSurname = model.NameSurname,
                    EmailConfirmed = true,
                    Role = "User"
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _notyf.Success("Kayıt Başarılı. Giriş yapabilirsiniz.");
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _notyf.Error(error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _notyf.Success("Çıkış Yapıldı");
            return RedirectToAction("Login", "Access");
        }

        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Access");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _notyf.Error("Google girişi başarısız oldu.");
                return RedirectToAction("Login");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _notyf.Success("Google ile giriş yapıldı");
                return RedirectToAction("Index", "Home");
            }

            // If user does not have an account, create one
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (email != null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    NameSurname = name,
                    EmailConfirmed = true,
                    Role = "User"
                };

                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _notyf.Success("Google ile kayıt olundu ve giriş yapıldı");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // If creation failed (maybe user exists but with different provider?), we can try to link?
                    // Or maybe just show error.
                    foreach (var err in createResult.Errors) _notyf.Error(err.Description);
                }
            }

            _notyf.Error("Giriş işlemi tamamlanamadı.");
            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new ProfileViewModel
            {
                NameSurname = user.NameSurname ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                CurrentProfilePhotoPath = user.ProfilePhotoPath
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.NameSurname = model.NameSurname;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.BirthDate = model.BirthDate;
            user.Gender = model.Gender;

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

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                _notyf.Success("Profil güncellendi.");
                return RedirectToAction("Profile");
            }

            _notyf.Error("Profil güncellenemedi.");
            return View(model);
        }
    }
}
