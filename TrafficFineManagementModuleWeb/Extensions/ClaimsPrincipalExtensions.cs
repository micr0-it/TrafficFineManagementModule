using EntityLayer.Enums;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace TrafficFineManagementModuleWeb.Extensions
{

    /// <summary>
    /// Controller’larda giriş yapan kullanıcının Id’sine ve rolüne sürekli ihtiyacımız olacak 
    /// (ApproveAsync(fineId, userId, role, ...) gibi). 
    /// Her seferinde claim’lerle uğraşmamak için extension metotlar yazdım.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        public const string NameSurnameClaim = "NameSurname";
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
        public static UserRole GetUserRole(this ClaimsPrincipal user) 
        {
            return Enum.Parse<UserRole>(user.FindFirstValue(ClaimTypes.Role)!);
        }
        public static string GetNameSurname(this ClaimsPrincipal user) 
        {
            return user.FindFirstValue(NameSurnameClaim) ?? user.Identity?.Name ?? string.Empty;
        }
    }
}
