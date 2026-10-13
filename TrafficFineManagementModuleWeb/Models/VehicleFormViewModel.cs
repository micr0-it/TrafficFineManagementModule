using EntityLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagementModuleWeb.Models
{
    public class VehicleFormViewModel
    {
        public int VehicleId { get; set; }
        [Required(ErrorMessage = "Plaka en fazla 15 Karakter Olabilir")]
        [Display(Name = "Plaka")]
        public string VehiclePlate { get; set; } = string.Empty;


        /// <summary>
        /// Vehicle Type Nullable olmasının nedeni
        /// Enum nullable olmazsa varsayılan değer olarak her zaman ilk elemanı alırdı(Passenger Binek araç)
        /// kullanıcı hiçbir şey seçmese bile form “Binek” seçilmiş gibi geçerdi ve [Required] hiç tetiklenmezdi. 
        /// Nullable yapınca “seçilmedi” durumu null olur ve validation çalışır.
        /// </summary>
        [Required(ErrorMessage = "Araç tipi seçilmedilir")]
        [Display(Name = "Araç Tipi")]
        public VehicleType? VehicleType { get; set; }
        
        [Required(ErrorMessage = "Marka Zorunludur.")]
        [StringLength(50, ErrorMessage ="Marka en fazla 50 karakter olabilir.")]
        [Display(Name = "Marka")]
        public string VehicleBrand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model Zorunludur.")]
        [StringLength(50, ErrorMessage ="Model en fazla 50 karakter olabilir.")]
        [Display(Name = "Model")]
        public string VehicleModel { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage ="Ruhsat seri no en fazla 20 karakter olabilir.")]
        [Display(Name = "Ruhsat Seri No")]
        public string? VehicleLicenceSerialNo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Tescil Tarihi")]
        public DateTime? VehicleRegistrationDate { get; set; }

        public bool IsEdit => VehicleId > 0;
    }
}
