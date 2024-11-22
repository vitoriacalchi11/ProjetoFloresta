using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoFloresta.Controllers
{
    public class PerfilController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public PerfilController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                // Verifica se a claim de telefone existe; se não, adiciona e atualiza o sign-in    
                var claims = await _userManager.GetClaimsAsync(user);
                if (!claims.Any(c => c.Type == ClaimTypes.MobilePhone) && !string.IsNullOrEmpty(user.PhoneNumber))
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
                    await _signInManager.RefreshSignInAsync(user); // Atualiza o sign-in para incluir a nova claim
                }

                ViewData["UserName"] = User.Identity.Name;
                ViewData["PhoneNumber"] = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
                ViewData["Email"] = User.FindFirst(ClaimTypes.Email)?.Value;

                return View();
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}
