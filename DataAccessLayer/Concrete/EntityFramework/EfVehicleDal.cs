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

        public Task<Vehicle?> GetByPlateAsync(string plate)
        {
            throw new NotImplementedException();
        }
    }
}
