using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Exceptions;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagementModuleWeb.Models;

namespace TrafficFineManagementModuleWeb.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(VehicleType? type, string? plate)
        {
            var plateSearch = string.IsNullOrWhiteSpace(plate) ? null : VehicleManager.NormalizePlate(plate);

            var vehicles = await _vehicleService.GetListAsync(v => (type == null || v.VehicleType == type) && (plateSearch == null || v.VehiclePlate.Contains(plateSearch)));

            var model = new VehicleListViewModel
            {
                Vehicles = vehicles.OrderBy(v => v.VehiclePlate).ToList(),
                TypeFilter = type,
                PlateSearch = plate
            };
            return View(model);
        }
        
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Create()
        {
            return View("Form", new VehicleFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Create(VehicleFormViewModel viewModel) 
        {
            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }

            var vehicle = new Vehicle();
            MapToEntity(viewModel, vehicle);

            try
            {
                await _vehicleService.AddAsync(vehicle);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Form", viewModel);
            }
            TempData["Success"] = $"{vehicle.VehiclePlate} plakalı araç eklendi.";
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if(vehicle is null)
            {
                return NotFound();
            }

            var model = new VehicleFormViewModel
            {
                VehicleId = vehicle.VehicleId,
                VehiclePlate = vehicle.VehiclePlate,
                VehicleType = vehicle.VehicleType,
                VehicleBrand = vehicle.VehicleBrand,
                VehicleModel = vehicle.VehicleModel,
                VehicleLicenceSerialNo = vehicle.VehicleLicenceSerialNo,
                VehicleRegistrationDate = vehicle.VehicleRegistrationDate
            };
            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Edit(int id, VehicleFormViewModel viewModel)
        {
            //if (id != model.VehicleId) return BadRequest();:
            //URL’deki Id ile formdaki gizli Id’nin uyuşmadığı,
            //yani biri formla oynamaya çalıştığı durum.
            if (id != viewModel.VehicleId)
            { 
                return BadRequest(); 
            }
            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle is null)
            {
                return NotFound();
            }
            MapToEntity(viewModel, vehicle);
            try
            {
                await _vehicleService.UpdateAsync(vehicle);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Form", viewModel);
            }
            TempData["Success"] = $"{vehicle.VehiclePlate} plakalı araç güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        //ViewModelimizi db için entity çevirdik
        private static void MapToEntity(VehicleFormViewModel model, Vehicle vehicle)
        {
            vehicle.VehiclePlate = model.VehiclePlate;
            vehicle.VehicleType = model.VehicleType!.Value;
            vehicle.VehicleBrand = model.VehicleBrand;
            vehicle.VehicleModel = model.VehicleModel;
            vehicle.VehicleLicenceSerialNo = string.IsNullOrWhiteSpace(model.VehicleLicenceSerialNo)
                ? null
                : model.VehicleLicenceSerialNo.Trim();
            vehicle.VehicleRegistrationDate = model.VehicleRegistrationDate;
        }
    }
}

