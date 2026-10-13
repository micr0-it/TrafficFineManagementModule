using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }
        public string VehiclePlate { get; set; } = null!;
        public VehicleType VehicleType { get; set; }
        public string VehicleModel { get; set; } = null!;
        public string VehicleBrand { get; set; } = null!;
        public string? VehicleLicenceSerialNo { get; set; }

        //Ruhsat sahibi alanlarımızı eklemiştim fakat kaldırdım
        //filodaki araçların ruhsat sahibi zaten şirket.
        
        public DateTime? VehicleRegistrationDate { get; set; }
        public ICollection<TrafficFine> VehicleFines { get; set; } = new List<TrafficFine>();

    }
}
