using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrafficFineManagementModuleWeb.Extensions;
using TrafficFineManagementModuleWeb.Models;

namespace TrafficFineManagementModuleWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppUserService _userService;

        public AccountController(IAppUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if(User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        //Rol claim’ini user.Role.ToString() ile yazdığımız için (“Admin”, “Manager”...), ileride controller’larda [Authorize(Roles = "Manager")] doğrudan çalışacak.

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]//Formun gerçekten bizim sayfamızdan gönderildiğini doğrular (CSRF koruması).
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userService.ValidateLoginAsync(viewModel.UserName, viewModel.Pwd);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(viewModel);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.AppUserId.ToString()),
                new(ClaimTypes.Name, user.AppUserName),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(ClaimsPrincipalExtensions.NameSurnameClaim, user.NameSurname)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties { IsPersistent = viewModel.RememberMe };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            //Sadece kendi sitemizdeki adreslere yönlendir
            //Login sonrası returnUrl’e yönlendiriyoruz. Bu kontrol olmazsa biri ?returnUrl=https://kotu-site.com içeren bir link göndererek kullanıcıyı giriş sonrası başka bir siteye yönlendirebilir (open redirect).
            if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
            { 
                return LocalRedirect(viewModel.ReturnUrl); 
            }

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]//GET olsaydı bir resim etiketi veya link ile kullanıcıyı habersizce çıkış yaptırmak mümkün olurdu.
        [ValidateAntiForgeryToken] //Formun gerçekten bizim sayfamızdan gönderildiğini doğrular (CSRF koruması).
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
