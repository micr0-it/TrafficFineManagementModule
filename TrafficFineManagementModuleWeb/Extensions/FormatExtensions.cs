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


        //Araştırmama göre
        //TC kimlik numarası KVKK kapsamında kişisel veri. Listede tamamen göstermek yerine maskelinmesi gerekirmiş; tam numara sadece düzenleme formunda görünecek.
        //Bu sayede prensiplere bağlı kalıyoruz.
        //Kontrolünün nasıl yapıldığını buldum ve ekledim
        
        /// <summary>
        /// Kişisel verinin listelerde açıkça görünmemesi için TC no maskelenir. Ör: 123******45
        /// </summary>
        public static string MaskIdentityNo(this string identityNo)
        {
            return identityNo.Length == 11 ? $"{identityNo[..3]}******{identityNo[^2..]}" : identityNo;
        }           
    }
}
