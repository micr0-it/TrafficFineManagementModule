using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfVehicleDal : GenericRepository<Vehicle>, IVehicleDal
    {
        public EfVehicleDal(AppDbContext context) : base(context) { }

        public async Task<Vehicle?> GetByPlateAsync(string plate)
        {
            return await _dbSet.Include(v => v.VehicleLicenseHolder).FirstOrDefaultAsync(v => v.VehiclePlate == plate);
        }

        public async Task<List<Vehicle>> GetListWithLicenseHolderAsync()
        {
            return await _dbSet.Include(v => v.VehicleLicenseHolder).ToListAsync();
        }
    }
}
