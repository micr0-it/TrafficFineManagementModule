using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    /// <summary>
    /// Şirkette araç kullanan çalışan. Ceza anında aracı kimin kullandığını belirtmek için kullanılır.
    /// </summary>
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }
        public string DriverNameSurname { get; set; } = null!;
        public string NationIdentityNo{ get; set; } = null!;
        /// <summary>
        /// Ehliyet bilgileri opsiyoneldir. Kayıt sadece ruhsat sahibi olarak
        /// tutuluyorsa (ör. şirket veya ehliyeti olmayan kişi) boş bırakılabilir.
        /// </summary>
        public string? DriverLicenceNo{ get; set; }
        public string? DriverLicenceClass{ get; set; }
        
        //Puan Hesabı attribute eklemiştim kaldırdım.
        public ICollection<TrafficFine> TrafficFines{ get; set; } = new List<TrafficFine>();
    }
}
