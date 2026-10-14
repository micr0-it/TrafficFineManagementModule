using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    /// <summary>
    /// Ceza listesindeki filtreler. Boş bırakılan alanlar filtreye dahil edilmez.
    /// Bu sınıf ne entity ne ViewModel: katmanlar arasında veri taşıyan basit bir nesne.
    /// WebUI’ye ait bir ViewModel’i Business’a göndermek katman kuralını bozacağı için
    /// (Business, WebUI’yi tanımamalı) Business’a kendi kriter sınıfını tanımladım.
    /// </summary>
    public class TrafficFineSearchCriteria
    {
        public ApprovalStatus? Status{ get; set; }
        public VehicleType? VehicleType { get; set; }

        /// <summary>
        /// Tutanak numarasında veya plakada aranır.
        /// </summary>
        public string? SearchText { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
