using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagementModuleWeb.Models
{
    public class TrafficFineListViewModel
    {
        // Filtreler (adres çubuğundan gelir)
        [Display(Name = "Durum")]
        public ApprovalStatus? Status { get; set; }

        [Display(Name = "Araç Tipi")]
        public VehicleType? VehicleType { get; set; }

        [Display(Name = "Tutanak No / Plaka")]
        public string? SearchText { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç")]
        public DateTime? DateFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Bitiş")]
        public DateTime? DateTo { get; set; }

        // Sonuçlar
        [ValidateNever]
        public List<TrafficFine> Fines { get; set; } = new();

        public decimal TotalAmount => Fines.Sum(f => f.TrafficFineAmount);
    }
}
