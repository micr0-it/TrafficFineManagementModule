using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfTrafficFineDal : GenericRepository<TrafficFine>, ITrafficFineDal
    {
        public EfTrafficFineDal(AppDbContext context) : base(context) { }

        private IQueryable<TrafficFine> DetailedQuery()
        {
            return _dbSet.Include(f => f.Vehicle).ThenInclude(v => v.VehicleLicenseHolder)
                .Include(f => f.Driver)
                .Include(f => f.Driver)
                .Include(f => f.TrafficFineType)
                .Include(f => f.RecordedByPersonnel);
        }
        public async Task<TrafficFine?> GetByIdWithDetailsAsync(int id)
        {
            return await DetailedQuery().FirstOrDefaultAsync(f => f.TrafficFineId == id);
        }

        public async Task<List<TrafficFine>> GetByPersonnelAsync(int personnelId)
        {
            return await DetailedQuery().Where(f => f.RecordedByPersonnelId == personnelId)
                .OrderBy(f => f.TrafficFineDate)
                .ToListAsync();
        }

        public async Task<List<TrafficFine>> GetListWithDetailsAsync()
        {
            return await DetailedQuery().OrderByDescending(f => f.TrafficFineDate).ToListAsync();
        }

        public async Task<List<TrafficFine>> GetPendingApprovalAsync()
        {
            return await DetailedQuery().Where(f => f.ApprovalStatus == ApprovalStatus.PendingManagerApproval)
                .OrderBy(f => f.TrafficFineDate)
                .ToListAsync();
        }
    }
}
