using EntityLayer.Concrete;
using EntityLayer.Enums;

namespace TrafficFineManagementModuleWeb.Models
{
    public class VehicleListViewModel
    {
        public List<Vehicle> Vehicles { get; set; } = new();
        public VehicleType? TypeFilter { get; set; }
        public string? PlateSearch { get; set; }
    }
}
