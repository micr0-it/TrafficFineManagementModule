using BusinessLayer.Workflow;
using EntityLayer.Concrete;
using EntityLayer.Enums;

namespace TrafficFineManagementModuleWeb.Models
{
    public class TrafficFineDetailsViewModel
    {
        public TrafficFine Fine { get; set; } = null!;

        /// <summary>
        /// Cezanın bulunduğu aşamanın tanımı. Süreç bittiyse (Tamamlandı / Reddedildi) null.
        /// </summary>
        public ApprovalStep? CurrentStep { get; set; }

        public bool CanApprove { get; set; }
        public bool CanReject { get; set; }

        public bool HasAction => CanApprove || CanReject;

        /// <summary>
        /// "Yeni" aşamasında onay butonu "Onaya Gönder", diğer aşamalarda "Onayla" olarak görünür.
        /// </summary>
        public string ApproveButtonText =>  CurrentStep?.ApproveActionType == ApprovalActionType.Submitted ? "Onaya Gönder" : "Onayla";
    }
}
