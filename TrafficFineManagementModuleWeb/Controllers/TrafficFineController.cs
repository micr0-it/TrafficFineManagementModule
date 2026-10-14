using BusinessLayer.Abstract;
using BusinessLayer.Exceptions;
using BusinessLayer.Models;
using BusinessLayer.Workflow;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using TrafficFineManagementModuleWeb.Extensions;
using TrafficFineManagementModuleWeb.Models;
using static System.Net.WebRequestMethods;

namespace TrafficFineManagementModuleWeb.Controllers
{
    public class TrafficFineController : Controller
    {
        // Onay/ret işlemlerini yapabilecek roller: "Manager,Finance"
        private const string ApproverRoles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Finance)}";
        private readonly ITrafficFineService _trafficFineService;
        private readonly IVehicleService _vehicleService;
        private readonly IDriverService _driverService;
        private readonly ITrafficFineTypeService _trafficFineTypeService;

        public TrafficFineController(ITrafficFineService trafficFineService, IVehicleService vehicleService, IDriverService driverService, ITrafficFineTypeService trafficFineTypeService)
        {
            _trafficFineService = trafficFineService;
            _vehicleService = vehicleService;
            _driverService = driverService;
            _trafficFineTypeService = trafficFineTypeService;
        }


        // ---------- Tüm roller: genel liste ----------

        [HttpGet]
        public async Task<IActionResult> Index(TrafficFineListViewModel model)
        {
            var criteria = new TrafficFineSearchCriteria
            {
                Status = model.Status,
                VehicleType = model.VehicleType,
                SearchText = model.SearchText,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            };

            try
            {
                model.Fines = await _trafficFineService.SearchAsync(criteria);
            }
            catch (BusinessException ex)
            {
                // Filtre hatası sayfayı bozmasın; mesaj gösterilir, liste boş kalır
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var trafficFine = await _trafficFineService.GetByIdWithDetailsAsync(id);
            if (trafficFine is null)
            {
                return NotFound();
            }

            var role = User.GetUserRole();

            // "Yeni" aşamasında onaya gönderme işlemi sadece Cezalarım ekranından (Submit) yapılır,
            // bu yüzden detay sayfasında personele onay butonu gösterilmez.
            var canApprove = ApprovalWorkFlow.CanApprove(trafficFine.ApprovalStatus, role)
                             && role != UserRole.Personnel;

            var viewModel = new TrafficFineDetailsViewModel
            {
                Fine = trafficFine,
                CurrentStep = ApprovalWorkFlow.GetStep(trafficFine.ApprovalStatus),
                CanApprove = canApprove,
                CanReject = ApprovalWorkFlow.CanReject(trafficFine.ApprovalStatus, role)
            };

            return View(viewModel);
        }


        // ---------- Yönetici ve Finans: bekleyen işler ----------

        [HttpGet]
        [Authorize(Roles = ApproverRoles)]
        public async Task<IActionResult> Pending()
        {
            var trafficFines = await _trafficFineService.GetActionableFinesAsync(User.GetUserId(), User.GetUserRole());
            return View(trafficFines);
        }

        /// <summary>
        /// Cezayı bulunduğu aşamada onaylar. Hangi aşamaya geçileceğine ApprovalWorkflow karar verir.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApproverRoles)]
        public async Task<IActionResult> Approve(int id, string? description)
        {
            try
            {
                await _trafficFineService.ApproveAsync(id, User.GetUserId(), User.GetUserRole(), description);
                TempData["Success"] = "Ceza onaylandı ve bir sonraki aşamaya iletildi.";
                return RedirectToAction(nameof(Pending));
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        /// <summary>
        /// Cezayı reddeder. Ret nedeni zorunludur (Business katmanında kontrol edilir).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApproverRoles)]
        public async Task<IActionResult> Reject(int id, string? description)
        {
            try
            {
                await _trafficFineService.RejectAsync(id, User.GetUserId(), User.GetUserRole(), description ?? string.Empty);
                TempData["Success"] = "Ceza reddedildi.";
                return RedirectToAction(nameof(Pending));
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
        }



        // ---------- Personel: kendi cezaları ----------

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Personnel))]
        public async Task<IActionResult> MyFines()
        {
            var trafficFines = await _trafficFineService.GetByPersonnelAsync(User.GetUserId());
            return View(trafficFines);
        }


        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Personnel))]
        public async Task<IActionResult> Create()
        {
            var viewModel = new TrafficFineFormViewModel();
            await PopulateOptionsAsync(viewModel);
            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Personnel))]
        public async Task<IActionResult> Create(TrafficFineFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateOptionsAsync(viewModel);
                return View("Form", viewModel);
            }

            var trafficFine = new TrafficFine
            {
                RecordedByPersonnelId = User.GetUserId()
            };
            MapToEntity(viewModel, trafficFine);

            try
            {
                await _trafficFineService.AddAsync(trafficFine);
            }
            catch (BusinessException ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateOptionsAsync(viewModel);
                return View("Form", viewModel);
            }
            
            TempData["Success"] = $"{trafficFine.TrafficFineRecordNo} numaralı ceza kaydedildi. Onaya göndermeyi unutmayın.";
            return RedirectToAction(nameof(MyFines));

            
        }
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Personnel))]
        public async Task<IActionResult> Edit(int id)
        {
            var trafficFine = await _trafficFineService.GetByIdAsync(id);
            if (trafficFine is null)
            {
                return NotFound();
            }

            if (!CanModify(trafficFine))
            {
                TempData["Error"] = "Sadece kendi girdiğiniz ve henüz onaya gönderilmemiş cezaları düzenleyebilirsiniz.";
                return RedirectToAction(nameof(MyFines));
            }

            var model = new TrafficFineFormViewModel
            {
                TrafficFineId = trafficFine.TrafficFineId,
                TrafficFineRecordNo = trafficFine.TrafficFineRecordNo,
                TrafficFineDate = trafficFine.TrafficFineDate,
                DateOfNotification = trafficFine.DateOfNotification,
                VehicleId = trafficFine.VehicleId,
                TrafficFineTypeId = trafficFine.TrafficFineTypeId,
                DriverId = trafficFine.DriverId
            };

            await PopulateOptionsAsync(model);
            return View("Form", model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Personnel))]
        public async Task<IActionResult> Edit(int id, TrafficFineFormViewModel viewModel)
        {
            if (id != viewModel.TrafficFineId)
            { 
                return BadRequest(); 
            }

            var existing = await _trafficFineService.GetByIdAsync(id);
            if (existing is null)
            { 
                return NotFound(); 
            }

            if (!CanModify(existing))
            {
                TempData["Error"] = "Sadece kendi girdiğiniz ve henüz onaya gönderilmemiş cezaları düzenleyebilirsiniz.";
                return RedirectToAction(nameof(MyFines));
            }

            if (!ModelState.IsValid)
            {
                await PopulateOptionsAsync(viewModel);
                return View("Form", viewModel);
            }

            var fine = new TrafficFine { TrafficFineId = id };
            MapToEntity(viewModel, fine);

            try
            {
                await _trafficFineService.UpdateAsync(fine);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateOptionsAsync(viewModel);
                return View("Form", viewModel);
            }

            TempData["Success"] = $"{fine.TrafficFineRecordNo} numaralı ceza güncellendi.";
            return RedirectToAction(nameof(MyFines));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Personnel))]
        public async Task<IActionResult> Delete(int id)
        {
            var fine = await _trafficFineService.GetByIdAsync(id);
            if (fine is null)
                return NotFound();

            if (!CanModify(fine))
            {
                TempData["Error"] = "Sadece kendi girdiğiniz ve henüz onaya gönderilmemiş cezaları silebilirsiniz.";
                return RedirectToAction(nameof(MyFines));
            }

            try
            {
                await _trafficFineService.DeleteAsync(fine);
                TempData["Success"] = $"{fine.TrafficFineRecordNo} numaralı ceza silindi.";
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MyFines));

        }
        /// <summary>
        /// "Yeni" durumdaki cezayı yönetici onayına gönderir (onay akışının ilk adımı).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Personnel))]
        public async Task<IActionResult> Submit(int id)
        {
            try
            {
                await _trafficFineService.ApproveAsync(id, User.GetUserId(), User.GetUserRole(), description: null);
                TempData["Success"] = "Ceza yönetici onayına gönderildi.";
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MyFines));
        }







        // --- Bu Controller'da kullanacağım yardımcı metotlar ---


        /// <summary>
        /// Ceza sadece onu giren personel tarafından ve henüz onaya gönderilmemişken değiştirilebilir.
        /// </summary>

        private bool CanModify(TrafficFine trafficFine)
        {
            return trafficFine.RecordedByPersonnelId == User.GetUserId() && trafficFine.ApprovalStatus == ApprovalStatus.New;
        }
        private static void MapToEntity(TrafficFineFormViewModel viewModel, TrafficFine trafficFine)
        {
            trafficFine.TrafficFineRecordNo = viewModel.TrafficFineRecordNo.Trim();
            trafficFine.TrafficFineDate = viewModel.TrafficFineDate!.Value;
            trafficFine.DateOfNotification = viewModel.DateOfNotification;
            trafficFine.VehicleId = viewModel.VehicleId!.Value;
            trafficFine.TrafficFineTypeId = viewModel.TrafficFineTypeId!.Value;
            trafficFine.DriverId = viewModel.DriverId;
        }

        /// <summary>
        /// Dropdown'ları doldurur. Formda hata olup sayfa geri döndüğünde de çağrılmalıdır.
        /// Dropdown seçenekleri formla geri gelmediği için, validation hatasıyla form geri döndüğünde listeler boş olur.
        /// 
        /// </summary>

        private async Task PopulateOptionsAsync(TrafficFineFormViewModel viewModel)
        {
            var vehicles = await _vehicleService.GetListAsync();
            viewModel.VehicleOptions = vehicles.OrderBy(v=>v.VehiclePlate)
                .Select(v=> new SelectListItem($"{v.VehiclePlate} - {v.VehicleType.GetDisplayName()}({v.VehicleBrand} {v.VehicleModel})", v.VehicleId.ToString())).ToList();

            // Pasif türler listelenmez; ama düzenlenen cezanın mevcut türü pasifse o da gösterilir
            var selectedTypeId   = viewModel.TrafficFineTypeId;
            var trafficFineTypes = await _trafficFineTypeService.GetListAsync(t => t.IsActive || t.TrafficFineTypeId == selectedTypeId);
            viewModel.FineTypeOptions = trafficFineTypes.OrderBy(t=> t.Article).Select(t=> new SelectListItem($"{t.Article} - {t.Description} ({t.Amount.ToMoney()})",
                t.TrafficFineTypeId.ToString()))
            .ToList();
            
            var drivers = await _driverService.GetListAsync();
            viewModel.DriverOptions = drivers
                .OrderBy(d => d.DriverNameSurname)
                .Select(d => new SelectListItem(
                    $"{d.DriverNameSurname} ({d.DriverLicenceClass})",
                    d.DriverId.ToString()))
                .ToList();
        }

    }
}
