using EntityLayer.Concrete;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ITrafficFineDal : IGenericDal<TrafficFine>
    {
        Task<List<TrafficFine>> GetListWithDetailsAsync();
        Task<TrafficFine?> GetByIdWithDetailsAsync(int id);
        Task<List<TrafficFine>> GetByStatusAsync(ApprovalStatus status);

        Task<List<TrafficFine>> GetByPersonnelAsync(int personnelId);

    }
}
