using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagementModuleWeb.Models
{

    /// <summary>
    /// Entity yerine ViewModel kullanmamızın sebebi: 
    /// login formunda AppUser’ın alanlarına değil, 
    /// sadece kullanıcı adı ve şifreye ihtiyaç var. 
    /// Ayrıca form validation kuralları ([Required]) entity’yi kirletmeden burada duruyor. 
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Zorunludur!")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre Zorunludur!")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Pwd { get; set; } = string.Empty;
        
        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
