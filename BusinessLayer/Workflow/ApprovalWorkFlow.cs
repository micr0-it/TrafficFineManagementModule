using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Workflow
{
    /// <summary>
    /// Trafik cezası onay akışının tanımı:
    /// Yeni → Yönetici Onayı → Finans Onayı → Tamamlandı
    /// Yeni bir aşama eklemek için ApprovalStatus'a bir değer ve buraya bir satır eklemek yeterlidir.
    /// </summary>
    public static class ApprovalWorkFlow
    {
        private static readonly IReadOnlyDictionary<ApprovalStatus, ApprovalStep> Steps = new Dictionary<ApprovalStatus, ApprovalStep> {
            [ApprovalStatus.New] = new(
                ApprovalStatus.New,
                UserRole.Personnel,
                ApprovalStatus.PendingManagerApproval,
                ApprovalActionType.Submitted,
                CanBeRejected: false),
            [ApprovalStatus.PendingManagerApproval] = new(
                ApprovalStatus.PendingManagerApproval,
                UserRole.Manager,
                ApprovalStatus.PendingFinanceApproval,
                ApprovalActionType.Approved,
                CanBeRejected: true),
            [ApprovalStatus.PendingFinanceApproval] = new(
                ApprovalStatus.PendingFinanceApproval,
                UserRole.Finance,
                ApprovalStatus.Completed,
                ApprovalActionType.Approved,
                CanBeRejected: true)         
            };
        
        /// <summary>
        /// Durumun aşama tanımını döner. Tamamlanmış veya reddedilmiş cezalarda null döner.
        /// </summary>
        public static ApprovalStep? GetStep(ApprovalStatus status)
        {
            return Steps.TryGetValue(status, out var step) ? step : null;
        }
        public static bool IsFinal(ApprovalStatus status)
        {
            return status is ApprovalStatus.Completed or ApprovalStatus.Rejected;
        }
        public static bool CanApprove(ApprovalStatus status, UserRole role)
        {
            return GetStep(status)?.ResponsibleRole == role;
        }
        public static bool CanReject(ApprovalStatus status, UserRole role)
        {
            return GetStep(status) is { CanBeRejected: true } step && step.ResponsibleRole == role;
        }

        /// <summary>
        /// Rolün işlem yapabileceği durumlar. "Bekleyen işlerim" ekranı için kullanılır.
        /// </summary>
        public static IReadOnlyList<ApprovalStatus> GetActionableStatuses(UserRole role) 
        {
            return Steps.Values.Where(s => s.ResponsibleRole == role)
                .Select(s => s.CurrentStatus)
                .ToList();
        }

        ///<summary>
        ///CanApprove ve CanReject metotlarını view’larda da kullanacağım: 
        ///kullanıcının rolü o aşamaya uygun değilse Onayla/Reddet butonlarını hiç göstermeyeceğim. 
        ///Kural yine tek yerde, 
        ///hem ekran hem iş katmanı aynı tabloya bakıyor.
        ///</summary>
    }
}
