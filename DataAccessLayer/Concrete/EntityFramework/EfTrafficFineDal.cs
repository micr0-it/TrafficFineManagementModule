using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfTrafficFineDal : GenericRepository<TrafficFine>, ITrafficFineDal
    {
        public EfTrafficFineDal(AppDbContext context) : base(context) { }

        private IQueryable<TrafficFine> DetailedQuery()
        {
            return _dbSet.Include(f => f.Vehicle)
                .Include(f => f.Driver)
                .Include(f => f.Driver)
                .Include(f => f.TrafficFineType)
                .Include(f => f.RecordedByPersonnel);
        }

        /// <summary>
        /// Detay sayfası için: onay geçmişi ve işlemi yapan kullanıcılar da yüklenir.
        /// </summary>

        /// <summary>
        /// Liste sayflarında geçmişi yüklemeyeceğim, sadece detay sayfasında yükleyeceğim
        /// Her listede geçmişi yüklemem gereksiz yük bindirecek
        /// Include İçinde OrderBy kullanarak geçmiş tarihleri sırayla çağırdım.
        /// </summary>
        public async Task<TrafficFine?> GetByIdWithDetailsAsync(int id)
        {
            return await DetailedQuery()
                .Include(f => f.ApprovalHistories
                .OrderBy(h => h.ActionDate))
                .ThenInclude(h => h.PerformedByUser)
                .FirstOrDefaultAsync(f => f.TrafficFineId == id);
        }
                
        public async Task<List<TrafficFine>> GetByPersonnelAsync(int personnelId)
        {
            return await DetailedQuery()
                .Where(f => f.RecordedByPersonnelId == personnelId)
                .OrderBy(f => f.TrafficFineDate)
                .ToListAsync();
        }

        public async Task<List<TrafficFine>> GetListWithDetailsAsync(Expression<Func<TrafficFine, bool>>? filter = null)
        {
            //Interface de belirttiğim gibi buraya ekledik where sorgusunu
            var query = DetailedQuery();

            if (filter is not null)
                query = query.Where(filter);

            return await query
                .OrderByDescending(f => f.TrafficFineDate)
                .ToListAsync();
        }

        public async Task<List<TrafficFine>> GetPendingApprovalAsync()
        {
            return await DetailedQuery()
                .Where(f => f.ApprovalStatus == ApprovalStatus.PendingManagerApproval)
                .OrderBy(f => f.TrafficFineDate)
                .ToListAsync();
        }

        public async Task<List<TrafficFine>> GetByStatusAsync(ApprovalStatus status)
        {
            return await DetailedQuery()
                .Where(f => f.ApprovalStatus == status)
                .OrderBy(f => f.TrafficFineDate)
                .ToListAsync();
        }
    }
}
