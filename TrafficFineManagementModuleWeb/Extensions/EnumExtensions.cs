using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TrafficFineManagementModuleWeb.Extensions
{
    /// <summary>
    /// Enum’lara eklediğim 
    /// [Display(Name = "Yönetici Onayında")] gibi Türkçe isimleri ekranda göstermek için:
    /// </summary>
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName() ?? value.ToString();
        }
    }
}
