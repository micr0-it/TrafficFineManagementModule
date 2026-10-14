using BusinessLayer.Abstract;
using BusinessLayer.Exceptions;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TrafficFineManagementModuleWeb.Models;

namespace TrafficFineManagementModuleWeb.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class DriverController : Controller
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var term = search?.Trim();

            var drivers = await _driverService.GetListAsync(d => string.IsNullOrEmpty(term) || d.DriverNameSurname.Contains(term));

            ViewData["Search"] = search;
            return View(drivers.OrderBy(d => d.DriverNameSurname).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form", new DriverFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DriverFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }

            var driver = new Driver();
            MapToEntity(viewModel, driver);

            try
            {
                await _driverService.AddAsync(driver);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Form", viewModel);
            }
            TempData["Success"] = $"{driver.DriverNameSurname} eklendi.";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var driver = await _driverService.GetByIdAsync(id);
            if (driver is null)
            {
                return NotFound();
            }

            var model = new DriverFormViewModel
            {
                DriverId = driver.DriverId,
                DriverNameSurname = driver.DriverNameSurname,
                NationIdentityNo = driver.NationIdentityNo,
                DriverLicenceNo = driver.DriverLicenceNo,
                DriverLicenceClass = driver.DriverLicenceClass
            };

            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DriverFormViewModel viewModel)
        {
            if (id != viewModel.DriverId)
            { 
                return BadRequest(); 
            }

            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }

            var driver = await _driverService.GetByIdAsync(id);
            if (driver is null)
            { return NotFound(); }
            
            MapToEntity(viewModel, driver);

            try
            {
                await _driverService.UpdateAsync(driver);
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Form", viewModel);
            }
            TempData["Success"] = $"{driver.DriverNameSurname} güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        private static void MapToEntity(DriverFormViewModel model, Driver driver)
        {
            driver.DriverNameSurname = model.DriverNameSurname.Trim();
            driver.NationIdentityNo = model.NationIdentityNo.Trim();
            driver.DriverLicenceNo = model.DriverLicenceNo.Trim();
            driver.DriverLicenceClass = model.DriverLicenceClass;
        }
    }
}
