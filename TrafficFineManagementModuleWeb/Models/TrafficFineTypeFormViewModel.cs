using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagementModuleWeb.Models
{
    public class TrafficFineTypeFormViewModel
    {
        public int TrafficFineTypeId { get; set; }

        [Required(ErrorMessage = "Kanun maddesi girilmesi zorunludur.")]
        [StringLength(30, ErrorMessage = "Kanun maddesi en fazla 30 karakter olabilir.")]
        [Display(Name = "Kanun Maddesi")]
        public string Article { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Açıklama  girilmesi zorunludur.")]
        [StringLength(250, ErrorMessage = "Açıklama en fazla 250 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tutar zorunludur.")]
        [Range(typeof(decimal), "0.01", "1000000",
        ParseLimitsInInvariantCulture = true,
        ConvertValueInInvariantCulture = true,
        ErrorMessage = "Tutar 0,01 ile 1.000.000 arasında olmalıdır.")]
        [Display(Name = "Tutar (₺)")]
        //Nullable olması sebebi boş bırakılmamalı, varsayılan 0 olduğundan required tetiklenmez
        //Required'ın tetiklenmesi için ? kontrolü yaptım
        //Nullable yapınca boş gerçekten boştur, aksi halde 0 yazar geçer
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Ceza puanı zorunludur.")]
        [Range(0, 100, ErrorMessage = "Ceza puanı 0 ile 100 arasında olmalıdır.")]
        [Display(Name = "Ceza Puanı")]
        //Nullable olması sebebi boş bırakılmamalı, varsayılan 0 olduğundan required tetiklenmez
        //Required'ın tetiklenmesi için ? kontrolü yaptık
        //Nullable yapınca boş gerçekten boştur, aksi halde 0 yazar geçer
        public int? TrafficFinePoint { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        public bool IsEdit => TrafficFineTypeId > 0;
    }
}
