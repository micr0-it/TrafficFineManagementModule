using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    /// <summary>
    /// Cezanın onay sürecindeki her işlemin kaydı. Kayıtlar sadece eklenir, değiştirilmez ve silinmez.
    /// </summary>
    public class TrafficFineApprovalHistory
    {
        [Key]
        public int TrafficFineApprovalHistoryId { get; set; }

        public int TrafficFineId { get; set; }
        public TrafficFine TrafficFine { get; set; } = null!;

        public int PerformedByUserId { get; set; }//işlemi kim yaptı?
        public AppUser PerformedByUser { get; set; } = null!;
        public DateTime ActionDate { get; set; }//işlemin tarihi
        public ApprovalActionType ActionType { get; set; }//işlem tipi
        /// <summary>
        /// İşlem açıklaması. Ret işleminde ret nedeni zorunludur.
        /// </summary>
        public string? Description{ get; set; }
        /// <summary>
        /// Kayıt oluşturulurken önceki durum yoktur, bu yüzden nullable.
        /// </summary>
        public ApprovalStatus? PreviousStatus { get; set; }
        public ApprovalStatus NewStatus { get; set; } 
    }
}
