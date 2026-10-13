using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class TrafficFineApprovalHistory
    {
        public int TrafficFineApprovalHistoryId { get; set; }

        public int TrafficFineId { get; set; }
        public TrafficFine TrafficFine { get; set; } = null!;

        public int PerformedByUserId { get; set; }//işlemi kim yaptı?
        public AppUser PerformedByUser { get; set; } = null!;

        public DateTime ActionDate { get; set; }//işlemin tarihi
        public ApprovalActionType MyProperty { get; set; }//işlem tipi
        public string? Description{ get; set; }//işlemle alakalı ret veya açıklama
        public ApprovalStatus? PreviousStatus { get; set; }//önceki durumu oluşturulma anında boş olacak önceden yazılmadı çünkü
        public ApprovalStatus NewStatus { get; set; } //artık olustu ve yeni durumu ne olacak
    }
}
