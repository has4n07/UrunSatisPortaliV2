using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrunSatisPortali.Models;
using UrunSatisPortali.ViewModels;

namespace UrunSatisPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, INotyfService notyf) : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly INotyfService _notyf = notyf;

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.RoleExistsAsync(model.Name))
                {
                    _notyf.Warning("Bu rol zaten mevcut.");
                    return View(model);
                }

                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    _notyf.Success("Rol başarıyla oluşturuldu.");
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    _notyf.Error(error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            return View(new RoleViewModel { Id = role.Id, Name = role.Name! });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null) return NotFound();

                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    _notyf.Success("Rol güncellendi.");
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    _notyf.Error(error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _notyf.Error("Rol bulunamadı.");
                return RedirectToAction("Index");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                _notyf.Success("Rol silindi.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _notyf.Error(error.Description);
                }
            }
            return RedirectToAction("Index");
        }

        // --- User Role Management ---

        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var model = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.NameSurname ?? user.UserName!,
                Email = user.Email!
            };

            foreach (var role in _roleManager.Roles)
            {
                model.Roles.Add(new RoleSelection
                {
                    RoleName = role.Name!,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name!)
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            foreach (var role in model.Roles)
            {
                if (role.IsSelected && !await _userManager.IsInRoleAsync(user, role.RoleName))
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
                else if (!role.IsSelected && await _userManager.IsInRoleAsync(user, role.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }

            _notyf.Success($"Kullanıcı ({user.Email}) rolleri güncellendi.");
            return RedirectToAction("UserList");
        }
    }
}
