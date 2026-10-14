using BusinessLayer.Abstract;
using BusinessLayer.Exceptions;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagementModuleWeb.Models;

namespace TrafficFineManagementModuleWeb.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))] 
    //Burada tüm işlemler admin tarafından olacağı için
    //Action Action değil direkt olarak controller bazlı yetkilendirme yaptım.
    public class TrafficFineTypeController : Controller
    {
        private readonly ITrafficFineTypeService _trafficFineTypeService;

        public TrafficFineTypeController(ITrafficFineTypeService trafficFineTypeService)
        {
            _trafficFineTypeService = trafficFineTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fineTypes = await _trafficFineTypeService.GetListAsync();
            var ordered = fineTypes.OrderByDescending(t => t.IsActive).ThenBy(t => t.Article).ToList();
            return View(ordered);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form", new TrafficFineTypeFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrafficFineTypeFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            { return View("Form", viewModel); }

            var fineType = new TrafficFineType();
            MapToEntity(viewModel, fineType);

            try
            {
                await _trafficFineTypeService.AddAsync(fineType);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Form", viewModel);
            }

            TempData["Success"] = $"{fineType.Article} ceza türü eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var fineType = await _trafficFineTypeService.GetByIdAsync(id);
            if (fineType is null)
            {
                return NotFound();
            }

            var model = new TrafficFineTypeFormViewModel
            {
                TrafficFineTypeId = fineType.TrafficFineTypeId,
                Article = fineType.Article,
                Description = fineType.Description,
                Amount = fineType.Amount,
                TrafficFinePoint = fineType.TrafficFinePoint,
                IsActive = fineType.IsActive
            };

            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrafficFineTypeFormViewModel viewModel, int id)
        {
            if (id != viewModel.TrafficFineTypeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            { return View("Form", viewModel); }

            var fineType = await _trafficFineTypeService.GetByIdAsync(id);
            if (fineType is null)
            {
                return NotFound();
            }

            MapToEntity(viewModel, fineType);

            try
            {
                await _trafficFineTypeService.UpdateAsync(fineType);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Form", viewModel);
            }

            TempData["Success"] = $"{fineType.Article} ceza türü güncellendi.";
            return RedirectToAction(nameof(Index));


        }

        /// <summary>
        /// Ceza türü silinmez, pasif yapılır (TrafficFineTypeManager.DeleteAsync).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {

            /*
             
            controller DeleteAsync çağırıyor ama TrafficFineTypeManager bunu override ettiği için kayıt silinmiyor, pasif yapılıyor. 
            Override mantığının ekrandaki karşılığı bu. Tekrar aktif yapmak için düzenleme formundaki “Aktif” kutusu kullanılabiliyor.
             
             */
            var fineType = await _trafficFineTypeService.GetByIdAsync(id);
            if (fineType is null)
                return NotFound();

            await _trafficFineTypeService.DeleteAsync(fineType);

            TempData["Success"] = $"{fineType.Article} ceza türü pasif yapıldı.";
            return RedirectToAction(nameof(Index));
        }

        private static void MapToEntity(TrafficFineTypeFormViewModel model, TrafficFineType fineType)
        {
            fineType.Article = model.Article.Trim();
            fineType.Description = model.Description.Trim();
            fineType.Amount = model.Amount!.Value;
            fineType.TrafficFinePoint = model.TrafficFinePoint!.Value;
            fineType.IsActive = model.IsActive;
        }
    }
}
