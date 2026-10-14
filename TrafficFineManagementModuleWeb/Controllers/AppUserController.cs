using BusinessLayer.Abstract;
using BusinessLayer.Exceptions;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrafficFineManagementModuleWeb.Extensions;
using TrafficFineManagementModuleWeb.Models;

namespace TrafficFineManagementModuleWeb.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AppUserController : Controller
    {
        private readonly IAppUserService _appUserService;

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appUsers = await _appUserService.GetListAsync();

            var orderedUsers = appUsers.OrderByDescending(u => u.IsActive).ThenBy(u => u.Role).ThenBy(u => u.NameSurname).ToList();
            return View(orderedUsers);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            var viewModel = new AppUserCreateViewModel();
            PopulateRoleOptions(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUserCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                PopulateRoleOptions(viewModel);
                return View(viewModel);
            }

            var appUser = new AppUser
            {
                AppUserName = viewModel.AppUserName,
                NameSurname = viewModel.NameSurname.Trim(),
                RegistrationNumber = viewModel.RegistrationNumber.Trim(),
                Role = viewModel.Role!.Value
            };

            try
            {
                await _appUserService.CreateUserAsync(appUser, viewModel.Password);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopulateRoleOptions(viewModel);
                return View(viewModel);
            }
            TempData["Success"] = $"{appUser.NameSurname} ({appUser.Role.GetDisplayName()}) kullanıcısı oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Kullanıcıyı pasif veya tekrar aktif yapar. Kullanıcılar silinmez; onay geçmişindeki kayıtlar korunur.
        /// </summary>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActiveStatus(int id, bool isActive)
        {
            if (id == User.GetUserId())
            {
                TempData["Error"] = "Kendi hesabınızın durumunu değiştiremezsiniz.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _appUserService.SetActiveStatusAsync(id, isActive);
                TempData["Success"] = isActive ? "Kullanıcı aktif yapıldı." : "Kullanıcı pasif yapıldı.";
            }
            catch (BusinessException ex)
            {

                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }


            // --- Bu Controller'da kullanacağım yardımcı metotlar ---

            /// <summary>
            /// Admin rolü bu ekrandan atanamaz, bu yüzden listeye eklenmez.
            /// </summary>

            private static void PopulateRoleOptions(AppUserCreateViewModel viewModel)
        {
            viewModel.RoleOptions = Enum.GetValues<UserRole>()
                .Where(r => r != UserRole.Admin)
                .Select(r => new SelectListItem(r.GetDisplayName(), ((int)r).ToString()))
                .ToList();
        }
    }
}

