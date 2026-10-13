using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Enums
{

    public enum VehicleType
    {

        /// <[Display]>
        /// attribute’u, ekranda dropdown’larda İngilizce enum adı yerine Türkçe karşılığının görünmesi için; 
        /// view’ları yazarken kullanacağız.
        /// </[Display]>
        [Display(Name = "Binek")]
        Passenger,
        
        [Display(Name = "Çekici")]
        Tractor,
        
        [Display(Name = "Dorse")]
        Trailer,
        
        [Display(Name = "Kiralık Araç")]
        Rental,
    }
}
