using BusinessLayer.Abstract;
using BusinessLayer.Exceptions;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class TrafficFineTypeManager : GenericManager<TrafficFineType>, ITrafficFineTypeService
    {
        private readonly ITrafficFineTypeDal _fineTypeDal;

        public TrafficFineTypeManager(ITrafficFineTypeDal fineTypeDal) : base(fineTypeDal)
        {
            _fineTypeDal = fineTypeDal;
        }

        public override async Task AddAsync(TrafficFineType fineType)
        {
            Validate(fineType);
            await _fineTypeDal.AddAsync(fineType);
        }

        public override async Task UpdateAsync(TrafficFineType fineType)
        {
            Validate(fineType);
            await _fineTypeDal.UpdateAsync(fineType);
        }

        /// <summary>
        /// Silmek yerine pasif yapılır; bu türle girilmiş eski cezalar bozulmasın.
        /// </summary>
        public override async Task DeleteAsync(TrafficFineType fineType)
        {
            fineType.IsActive = false;
            await _fineTypeDal.UpdateAsync(fineType);
        }

        private static void Validate(TrafficFineType fineType)
        {
            if (fineType.Amount <= 0)
                throw new BusinessException("Ceza tutarı sıfırdan büyük olmalıdır.");

            if (fineType.TrafficFinePoint < 0)
                throw new BusinessException("Ceza puanı negatif olamaz.");
        }
    
    }
}
