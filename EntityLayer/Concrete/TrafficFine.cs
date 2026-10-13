using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class TrafficFine
    {
        [Key]
        public int TrafficFineId { get; set; }
        public string TrafficFineRecordNo { get; set; } = null!;
        public DateTime TrafficFineDate { get; set; }
        public decimal TrafficFineAmount { get; set; }

        /// <summary>
        /// Cezanın şirkete tebliğ edildiği tarih.
        /// </summary>
        public DateTime? DateOfNotification { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        public int TrafficFineTypeId { get; set; }
        public TrafficFineType TrafficFineType { get; set; } = null!;

        /// <summary>
        /// Ceza anında aracı kullanan sürücü. Radar/EDS gibi sürücünün bilinmediği
        /// cezalarda boş kalabilir.
        /// </summary>
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }

        //Buradan sonrası onay süreci için gerekli propertieslere ait
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.New;
        public int RecordedByPersonnelId { get; set; }
        public AppUser RecordedByPersonnel { get; set; } = null!;
        public ICollection<TrafficFineApprovalHistory> ApprovalHistories { get; set; } = new List<TrafficFineApprovalHistory>();
    }
}
