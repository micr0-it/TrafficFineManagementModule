using System.Globalization;
namespace TrafficFineManagementModuleWeb.Extensions
{
    /// <summary>
    /// Ekranda tutar ve tarihlerin Türkçe biçimde gösterilmesi için.
    /// </summary>
    public static class FormatExtensions
    {
        private static readonly CultureInfo Turkish = new("tr-TR");
        public static string ToMoney(this decimal value) => value.ToString("N2", Turkish) + " ₺";

        public static string ToDate(this DateTime value) => value.ToString("dd.MM.yyyy");

        public static string ToDate(this DateTime? value) => value?.ToString("dd.MM.yyyy") ?? "-";

        public static string ToDateTime(this DateTime value) => value.ToString("dd.MM.yyyy HH:mm");
    }
}
