using EntityLayer.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagementModuleWeb.Models
{
    public class AppUserCreateViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Kullanıcı adında sadece harf (Türkçe karakter hariç), rakam, nokta, alt çizgi ve tire kullanılabilir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string AppUserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad soyad en fazla 100 karakter olabilir.")]
        [Display(Name = "Ad Soyad")]
        public string NameSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sicil numarası zorunludur.")]
        [StringLength(20, ErrorMessage = "Sicil numarası en fazla 20 karakter olabilir.")]
        [Display(Name = "Sicil No")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rol seçilmelidir.")]
        [Display(Name = "Rol")]
        public UserRole? Role { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor.")] //Compare : İki alanın aynı olup olmadığını kontrol eder; şifre tekrarı için standart yöntem.
        [DataType(DataType.Password)]
        [Display(Name = "Şifre (Tekrar)")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [ValidateNever]
        public List<SelectListItem> RoleOptions { get; set; } = new();
    }
}
