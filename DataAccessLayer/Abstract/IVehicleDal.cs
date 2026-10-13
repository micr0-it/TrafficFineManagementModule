using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IVehicleDal : IGenericDal<Vehicle>
    {
        Task<List<Vehicle>> GetListWithLicenseHolderAsync();
        Task<Vehicle?> GetByPlateAsync(string plate);
    }
}
