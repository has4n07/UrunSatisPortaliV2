// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using UrunSatisPortali.Models;


namespace UrunSatisPortali.Areas.Identity.Pages.Account.Manage

{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment; // <-- 1. YENİ EKLENEN DEĞİŞKEN

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment) // <-- 2. YENİ EKLENEN PARAMETRE
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment; // <-- 3. YENİ EKLENEN EŞLEŞTİRME
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
             
           
            [Display(Name = "Ad Soyad")]
            public string AdSoyad { get; set; }

            [Display(Name = "Profil Resmi")]
            public string ProfilResmiUrl { get; set; } // Resmin yolu (göstermek için)

            [Display(Name = "Profil Fotoğrafı Yükle")]
            public IFormFile ProfilDosyasi { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            

            Username = userName;

            Input = new InputModel
            {
                
                AdSoyad = user.AdSoyad,
                ProfilResmiUrl = user.ProfilResmiUrl
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // --- 1. AD SOYAD GÜNCELLEME ---
            // Sadece Input.AdSoyad DOLU gelirse güncelle. Boşsa dokunma.
            if (!string.IsNullOrEmpty(Input.AdSoyad) && Input.AdSoyad != user.AdSoyad)
            {
                user.AdSoyad = Input.AdSoyad;
            }

            // --- 2. PROFİL FOTOĞRAFI GÜNCELLEME ---
            if (Input.ProfilDosyasi != null)
            {
                // Dosya adı oluştur
                var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(Input.ProfilDosyasi.FileName);

                // Kayıt yolunu belirle (wwwroot/img/users/ klasörüne atalım)
                // (Önce bu klasörü oluşturman gerekebilir)
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "img", "users");

                if (!Directory.CreateDirectory(uploadsFolder).Exists)
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, dosyaAdi);

                // Dosyayı kaydet
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilDosyasi.CopyToAsync(stream);
                }

                // Veritabanına yolu yaz
                user.ProfilResmiUrl = "/img/users/" + dosyaAdi;
            }

            // Değişiklikleri veritabanına işle
            await _userManager.UpdateAsync(user);

            // Oturumu yenile (ki yeni resim hemen görünsün)
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Profiliniz güncellendi";
            return RedirectToPage();
        }
    }
}
