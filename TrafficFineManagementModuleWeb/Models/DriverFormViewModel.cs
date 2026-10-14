using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagementModuleWeb.Models
{
    public class DriverFormViewModel
    {
        /*
         Ehliyet sınıfını serbest metin yerine sabit bir listeden seçtiriyorum; “b”, “B sınıfı”, “B “ gibi tutarsız kayıtların önüne geçer.
         Filo senaryosunda çekici kullanan şoförler için CE sınıfı önemli olduğundan anlamlı bir alan.        
        */

        /// <summary>
        /// Türkiye'deki sürücü belgesi sınıfları.
        /// </summary>
        public static readonly string[] LicenceClasses =
        { "M", "A1", "A2", "A", 
            "B1", "B", "BE", 
            "C1", "C1E", "C", "CE", 
            "D1", "D1E", "D", "DE", 
            "F", 
            "G" 
        };

        public int DriverId { get; set; }
        [Required(ErrorMessage = "Ad Soyad girilmesi zorunludur.")]
        [StringLength(120, ErrorMessage = "Ad Soyad en fazla 120 karakter olabilir.")]
        [Display(Name = "Ad Soyad")]
        public string DriverNameSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tc kimlik numarası girilmesi zorunludur.")]
        //Burada sadece 11 rakam mı kontrolü var 
        //Çakışan numara kontrolü manager kısmında yapıldı bkz:DriverManager
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Tc kimlik numarası 11 haneli olmalıdır.")]
        [Display(Name = "Tc Kimlik No")]
        public string NationIdentityNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ehliyet numarası zorunludur.")]
        [StringLength(20, ErrorMessage = "Ehliyet numarası en fazla 20 karakter olabilir.")]
        [Display(Name = "Ehliyet No")]
        public string DriverLicenceNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ehliyet sınıfı seçilmelidir.")]
        [Display(Name = "Ehliyet Sınıfı")]
        public string DriverLicenceClass { get; set; } = string.Empty;

        public bool IsEdit => DriverId > 0;


    }
}
