using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfTrafficFineTypeDal : GenericRepository<TrafficFineType>, ITrafficFineTypeDal
    {
        public EfTrafficFineTypeDal(AppDbContext context) : base(context) { }
    
    }
}
