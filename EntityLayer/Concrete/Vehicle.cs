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
        public string VehicleModel { get; set; } = null!;
        public string VehicleBrand { get; set; } = null!;
        public string? VehicleLicenceSerialNo { get; set; }
        public DateTime? VehicleRegistrationDate { get; set; }

        /// <summary>
        /// Aracın ruhsat sahibi. Aracı kullanan sürücüyle aynı kişi olmak zorunda değildir.
        /// </summary>
        public int VehicleLicenseHolderId { get; set; }
        public Driver VehicleLicenseHolder { get; set; } = null!;
        public ICollection<TrafficFine> VehicleFines { get; set; } = new List<TrafficFine>();

    }
}
