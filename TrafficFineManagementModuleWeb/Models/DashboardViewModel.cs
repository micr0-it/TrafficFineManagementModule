using EntityLayer.Concrete;
using EntityLayer.Enums;

namespace TrafficFineManagementModuleWeb.Models
{
    public class DashboardViewModel
    {
        public Dictionary<ApprovalStatus, int> StatusCounts { get; set; } = new();
        public int TotalCount { get; set; }

        /// <summary>
        /// Onay sürecinden geçip tamamlanan cezaların toplam tutarı.
        /// </summary>
        public decimal CompletedAmount { get; set; }

        /// <summary>
        /// Yönetici veya finans onayında bekleyen cezaların toplam tutarı.
        /// </summary>
        public decimal InApprovalAmount { get; set; }

        /// <summary>
        /// Giriş yapan kullanıcının işlem yapması gereken ceza sayısı.
        /// </summary>
        public int MyActionCount { get; set; }

        public List<TrafficFine> RecentFines { get; set; } = new();

        public int GetCount(ApprovalStatus status)
        {
            return StatusCounts.TryGetValue(status, out var count) ? count : 0;
        }
    }
}
