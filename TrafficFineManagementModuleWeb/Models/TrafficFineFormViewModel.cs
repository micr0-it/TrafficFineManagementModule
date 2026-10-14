using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagementModuleWeb.Models
{
    public class TrafficFineFormViewModel
    {

        //Bu formda ceza tutar alanı olmayacak çünkü
        //Ceza tutarım business katmanında burada seçtiğim ceza türüne göre
        //Otomatikman gelecek bu sayede kullanıcı yanlış tutar giremeyecek.

        public int TrafficFineId { get; set; }

        [Required(ErrorMessage = "Tutanak numarası zorunludur.")]
        [StringLength(30, ErrorMessage = "Tutanak numarası en fazla 30 karakter olabilir.")]
        [Display(Name = "Tutanak No")]
        public string TrafficFineRecordNo { get; set; } = string.Empty;

        //Burada DataType.Date vermeme sebemimiz ise cezanın kesildiği saat önemli itirazlarda bu saat kullanılabilir
        //Ben o saate orada değildim.. gibi
        [Required(ErrorMessage = "Ceza tarihi zorunludur.")]
        [Display(Name = "Ceza Tarihi")]
        public DateTime? TrafficFineDate { get; set; }

        
        //Burada DataType.Date vermemizin sebebi tebliğ tarihinde saatin önemi olmasın istedim
        //Araştırma sonucu bu yapıyı kullanırsam sorunumun çözüleceğini öğrenmdim
        //DataType.Date sayesinde sadece tarihi aldık saat almadık
        [DataType(DataType.Date)] 
        [Display(Name = "Tebliğ Tarihi")]
        public DateTime? DateOfNotification { get; set; }

        [Required(ErrorMessage = "Araç seçilmelidir.")]
        [Display(Name = "Araç")]
        public int? VehicleId { get; set; }

        [Required(ErrorMessage = "Ceza türü seçilmelidir.")]
        [Display(Name = "Ceza Türü")]
        public int? TrafficFineTypeId { get; set; }

        [Display(Name = "Sürücü")]
        public int? DriverId { get; set; }

        // Dropdown seçenekleri (formdan gelmez, controller doldurur)

        /// <summary>
        /// Neden ValideNever
        /// Projede bu kısımda hata aldım form ne yaparsam yapayım kaydedilmiyordu
        /// Sebebini yapay zekaya sorunca bu çözümü önerdi
        /// Projede nullable reference types açık olduğu için, List<SelectListItem> gibi nullable olmayan her property ASP.NET Core tarafından otomatik olarak zorunlu sayılır.
        /// Dropdown seçenekleri formla birlikte geri gönderilmediği için bu alanlar her POST’ta boş gelir ve ModelState.IsValid hep false olur
        /// [ValidateNever] bu alanların validation’a hiç girmemesini sağlıyor.
        /// </summary>
        [ValidateNever]
        public List<SelectListItem> VehicleOptions { get; set; } = new();

        [ValidateNever]
        public List<SelectListItem> FineTypeOptions { get; set; } = new();

        [ValidateNever]
        public List<SelectListItem> DriverOptions { get; set; } = new();

        public bool IsEdit => TrafficFineId > 0;
    }
}
