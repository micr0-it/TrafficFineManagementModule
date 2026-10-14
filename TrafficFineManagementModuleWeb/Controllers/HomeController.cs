using BusinessLayer.Abstract;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrafficFineManagementModuleWeb.Extensions;
using TrafficFineManagementModuleWeb.Models;

namespace TrafficFineManagementModuleWeb.Controllers
{
    public class HomeController : Controller
    {
        private const int RecentFineCount = 5;
        private readonly ITrafficFineService _trafficFineService;

        public HomeController(ITrafficFineService trafficFineService)
        {
            _trafficFineService = trafficFineService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trafficFines = await _trafficFineService.GetListWithDetailsAsync();
            var viewModel = new DashboardViewModel
            {
                TotalCount = trafficFines.Count,
                StatusCounts = trafficFines
                    .GroupBy(f => f.ApprovalStatus)
                    .ToDictionary(g => g.Key, g => g.Count()),
                CompletedAmount = trafficFines
                    .Where(f => f.ApprovalStatus == ApprovalStatus.Completed)
                    .Sum(f => f.TrafficFineAmount),
                InApprovalAmount = trafficFines
                    .Where(f => f.ApprovalStatus == ApprovalStatus.PendingManagerApproval
                             || f.ApprovalStatus == ApprovalStatus.PendingFinanceApproval)
                    .Sum(f => f.TrafficFineAmount),
                RecentFines = trafficFines
                    .Take(RecentFineCount)
                    .ToList()
            };

            var role = User.GetUserRole();
            if (role != UserRole.Admin)
            {
                var actionableFines = await _trafficFineService.GetActionableFinesAsync(User.GetUserId(), role);
                viewModel.MyActionCount = actionableFines.Count;
            }

            return View(viewModel);
        }




        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 404, 400 gibi durum kodları için kullanıcı dostu sayfa.
        /// </summary>
        [AllowAnonymous]
        public IActionResult StatusCodePage(int code)
        {
            ViewData["StatusCode"] = code;
            return View();
        }
    }
}
