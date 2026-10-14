using EntityLayer.Concrete;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ITrafficFineDal : IGenericDal<TrafficFine>
    {
        /// <TasarımNotu>
        /// Filtrelemeyi controller’da değil Business’ta yapacağım.
        /// Controller sadece “kullanıcı şunları seçti” diye bir kriter nesnesi gönderecek, sorguyu kurmak Business’ın işi olacak.
        /// Filtreleme de bellekte değil, veritabanında (SQL WHERE olarak) çalışacak;
        /// kayıt sayısı arttığında tüm cezaları çekip sonra elemek yavaş olacağı için bu yöntem kabul edilebilir.
        /// </TasarımNotu>

        //Yukarıdaki sebepten ötürü buraya opsiyonel bir filtre parametresi ekledim opsiyonel olduğu için mevct çağrılar da sorun olmayacak
        Task<List<TrafficFine>> GetListWithDetailsAsync(Expression<Func<TrafficFine, bool>>? filter = null);
        Task<TrafficFine?> GetByIdWithDetailsAsync(int id);
        Task<List<TrafficFine>> GetByStatusAsync(ApprovalStatus status);

        Task<List<TrafficFine>> GetByPersonnelAsync(int personnelId);

    }
}
