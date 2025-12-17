using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrunSatisPortali.Repository;

namespace UrunSatisPortali.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IShoppingCartRepository _shoppingCartRepo;
        public ShoppingCartViewComponent(IShoppingCartRepository shoppingCartRepo)
        {
            _shoppingCartRepo = shoppingCartRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var count = _shoppingCartRepo.GetAll(u => u.ApplicationUserId == claim.Value).Count();
                return View(count);
            }
            else
            {
                return View(0);
            }
        }
    }
}
