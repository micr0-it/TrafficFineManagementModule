using BusinessLayer.Models;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ITrafficFineService : IGenericService<TrafficFine>
    {
        Task<List<TrafficFine>> GetListWithDetailsAsync();
        Task<TrafficFine?> GetByIdWithDetailsAsync(int id);
        Task<List<TrafficFine>> GetByPersonnelAsync(int personnelId);

        /// <summary>
        /// Kullanıcının kendi aşamasında bekleyen, işlem yapabileceği cezalar.
        /// </summary>
        Task<List<TrafficFine>> GetActionableFinesAsync(int userId, UserRole role);

        Task ApproveAsync(int fineId, int UserId, UserRole role, string? description);
        Task RejectAsync(int fineId, int UserId, UserRole role, string reason);

        //Veri taşıayn nesnemi sorgulamak için ekledim.
        Task<List<TrafficFine>> SearchAsync(TrafficFineSearchCriteria criteria);
    }
}
