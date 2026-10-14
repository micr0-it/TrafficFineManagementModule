using BusinessLayer.Abstract;
using BusinessLayer.Exceptions;
using BusinessLayer.Models;
using BusinessLayer.Workflow;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BusinessLayer.Concrete
{
    public class TrafficFineManager : GenericManager<TrafficFine>, ITrafficFineService
    {
        private const int DescriptionMaxLength = 500;
        private readonly ITrafficFineDal _trafficFineDal;
        private readonly ITrafficFineTypeDal _trafficFineTypeDal;
        private readonly IVehicleDal _vehicleDal;
        public TrafficFineManager(ITrafficFineDal trafficFineDal,ITrafficFineTypeDal trafficFineTypeDal,IVehicleDal vehicleDal) : base(trafficFineDal)
        {
            _trafficFineDal = trafficFineDal;
            _trafficFineTypeDal = trafficFineTypeDal;
            _vehicleDal = vehicleDal;
        }

        //Listelemeler - Sorgulamalar
        public async Task<List<TrafficFine>> GetActionableFinesAsync(int userId, UserRole role)
        {
            var result = new List<TrafficFine>();
            foreach (var status in ApprovalWorkFlow.GetActionableStatuses(role))
            {
                var fines = await _trafficFineDal.GetByStatusAsync(status);

                //Yeni aşanasında personel sadece kendi girdiği cezaları onaya gönderecek
                if (status == ApprovalStatus.New)
                    fines = fines.Where(f => f.RecordedByPersonnelId == userId).ToList();

                result.AddRange(fines);
            }
            return result;
        }
        public Task<TrafficFine?> GetByIdWithDetailsAsync(int id)
        {
            return _trafficFineDal.GetByIdWithDetailsAsync(id);
        }
        public Task<List<TrafficFine>> GetByPersonnelAsync(int personnelId)
        {
            return _trafficFineDal.GetByPersonnelAsync(personnelId);
        }
        public Task<List<TrafficFine>> GetListWithDetailsAsync()
        {
            return _trafficFineDal.GetListWithDetailsAsync();
        }


        public async Task<List<TrafficFine>> SearchAsync(TrafficFineSearchCriteria criteria)
        {

            /*
             
            Buradaki algoritmada yaptıklarım
            status == null || Kullanıcı bir filtreyi boş bıraktıysa koşulun ilk kısmı true olacak
            ve o filtre devre dışı olacak bu saydede birden fazla if kullanmamış olacağız
            Ef Core bşze bu yapıyla SQL çevrilirken boş olan filtreleri sorgudan cıkarma ayrıcalıgı tanıyor.

            Arama hem tutanak numarası hem de plaka ile yapılır
            Burada Normalize.Plate ile aramada 48 mug 48 yazsa bile 48MUG48 olacak

            Bitiş tarihi: Ceza tarihleri saat de içeriyor. Bitiş tarihi olarak 14.10.2026 seçilirse <= 14.10.2026 00:00 koşulu, 
            o gün saat 10:30'da kesilen cezayı dışarıda bırakırdı. “Bir sonraki günün başlangıcından küçük” demek tüm günü kapsıyor.
             
             
             */
            if (criteria.DateFrom is not null && criteria.DateTo is not null &&
                criteria.DateFrom.Value.Date > criteria.DateTo.Value.Date)
                throw new BusinessException("Başlangıç tarihi bitiş tarihinden sonra olamaz.");

            var status = criteria.Status;
            var vehicleType = criteria.VehicleType;

            var recordNoSearch = string.IsNullOrWhiteSpace(criteria.SearchText) ? null : criteria.SearchText.Trim();
            var plateSearch = recordNoSearch is null ? null : VehicleManager.NormalizePlate(recordNoSearch);

            var dateFrom = criteria.DateFrom?.Date;
            // Bitiş tarihi gün sonuna kadar dahil olsun diye bir sonraki günün başlangıcı kullanılır
            var dateToExclusive = criteria.DateTo?.Date.AddDays(1);

            return await _trafficFineDal.GetListWithDetailsAsync(f =>
                (status == null || f.ApprovalStatus == status) &&
                (vehicleType == null || f.Vehicle.VehicleType == vehicleType) &&
                (recordNoSearch == null ||
                    f.TrafficFineRecordNo.Contains(recordNoSearch) ||
                    f.Vehicle.VehiclePlate.Contains(plateSearch!)) &&
                (dateFrom == null || f.TrafficFineDate >= dateFrom) &&
                (dateToExclusive == null || f.TrafficFineDate < dateToExclusive));
        }

        //Crud İşlemleri

        /// <summary>
        /// Yeni ceza girişi. RecordedByPersonnelId controller'da giriş yapan kullanıcıdan atanmış olmalı.
        /// </summary>



        public override async Task AddAsync(TrafficFine fine) 
        {
            await ValidateFineAsync(fine);
            
            fine.ApprovalStatus = ApprovalStatus.New;
            
            fine.ApprovalHistories.Add(new TrafficFineApprovalHistory
            {
                PerformedByUserId = fine.RecordedByPersonnelId,
                ActionDate = DateTime.Now,
                ActionType = ApprovalActionType.Created,
                PreviousStatus = null,
                NewStatus = ApprovalStatus.New
            });

            await _trafficFineDal.AddAsync(fine);
        }

        /// <summary>
        /// Sadece "Yeni" durumdaki, henüz onaya gönderilmemiş cezalar düzenlenebilir.
        /// </summary>
        /// 


        /*
         
         UpdateAsync neden formdan gelen nesneyi doğrudan kaydetmiyor? 
        İki sebep: formdan gelen nesnede RecordedByPersonnelId veya ApprovalStatus değiştirilmiş olabilir, 
        sadece düzenlenebilir alanları kopyalayarak bunu engelliyoruz. 
        Ayrıca aynı Id’li ceza GetByIdAsync ile zaten yüklendiği için, ikinci bir nesneyi Update ile eklemeye çalışmak EF’te “another instance with the same key is already being tracked” hatası verirdi.
         
         */
        public override async Task UpdateAsync(TrafficFine fine)
        {
            var existing = await _trafficFineDal.GetByIdAsync(fine.TrafficFineId) ?? throw new BusinessException("Ceza Bulunamadı");
            
            if(existing.ApprovalStatus != ApprovalStatus.New)
            {
                throw new BusinessException("Onaya gönderilmiş ceza düzenlenemez.");
            }

            await ValidateFineAsync(fine);

            // Sadece düzenlenebilir alanlar kopyalanır; kaydı giren personel, durum ve geçmiş korunur
            existing.TrafficFineRecordNo = fine.TrafficFineRecordNo;
            existing.TrafficFineDate = fine.TrafficFineDate;
            existing.DateOfNotification = fine.DateOfNotification;
            existing.TrafficFineTypeId = fine.TrafficFineTypeId;
            existing.TrafficFineAmount = fine.TrafficFineAmount;
            existing.VehicleId = fine.VehicleId;
            existing.DriverId = fine.DriverId;

            await _trafficFineDal.UpdateAsync(fine);
        }

        public override async Task DeleteAsync(TrafficFine fine)
        {
            if (fine.ApprovalStatus != ApprovalStatus.New)
                throw new BusinessException("Onaya gönderilmiş ceza silinemez.");

            await _trafficFineDal.DeleteAsync(fine);
        }
        //Onay Süreci
        public async Task ApproveAsync(int fineId, int UserId, UserRole role, string? description)
        {
            var fine = await GetFineForActionAsync(fineId);
            var step = GetStepOrThrow(fine);

            if (!ApprovalWorkFlow.CanApprove(fine.ApprovalStatus, role))
            {
                throw new BusinessException("Bu aşamada işlem yapma yetkiniz yoktur");
            }
            if (step.CurrentStatus == ApprovalStatus.New && fine.RecordedByPersonnelId != UserId)
            {
                throw new BusinessException("Sadece kendi girdiğiniz cezaları onaya gönderebilirsiniz");
            }

            AddTransition(fine, UserId, step.NextStatus, step.ApproveActionType, description);
            await _trafficFineDal.UpdateAsync(fine);
        }
        
        public async Task RejectAsync(int fineId, int UserId, UserRole role, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new BusinessException("Ret nedeni yazılmalıdır.");
            }
            var fine = await GetFineForActionAsync(fineId);
            GetStepOrThrow(fine);

            if(!ApprovalWorkFlow.CanReject(fine.ApprovalStatus, role))
            {
                throw new BusinessException("Bu aşamada ret işlemi yapma hakkınız yok");
            }
            AddTransition(fine, UserId, ApprovalStatus.Rejected, ApprovalActionType.Rejected, reason);
            await _trafficFineDal.UpdateAsync(fine);
        }


        //Yukarıdaki metotlarda kullanmak üzere yazdığım yardımcı metotlar
        private async Task<TrafficFine> GetFineForActionAsync(int fineId)
        {
            return await _trafficFineDal.GetByIdAsync(fineId) ?? throw new BusinessException("Ceza Bulunamadı.");
        }

        private static ApprovalStep GetStepOrThrow(TrafficFine fine)
        {
            return ApprovalWorkFlow.GetStep(fine.ApprovalStatus) ?? throw new BusinessException("Bu ceza için onay süreci tamamlanmış, işlem yapılamaz.");
        }

        /// <summary>
        /// Durumu değiştirir ve geçmişe kayıt ekler. İkisi aynı SaveChanges ile kaydedilir.
        /// </summary>

        /*
         
         AddTransition: durum ve geçmiş birlikte değişir. 
        Durum değişikliği ile geçmiş kaydı aynı metotta yapılıyor; birini yapıp diğerini unutmak mümkün değil. 
        Geçmiş kaydı fine.ApprovalHistories.Add(...) ile cezanın üzerinden ekleniyor, 
        _fineDal.UpdateAsync(fine) içindeki tek SaveChangesAsync ikisini aynı transaction’da kaydediyor. 
        Bu yüzden geçmiş tablosu için ayrı bir DAL’a ihtiyaç duymadık.
         
         
         */

        private static void AddTransition(TrafficFine trafficFine, int userId, ApprovalStatus newStatus, ApprovalActionType actionType, string? description)
        {
            description = description?.Trim();
            if (description?.Length > DescriptionMaxLength)
            {
                throw new BusinessException($"Açıklama uzunluğunuz en fazla {DescriptionMaxLength} karakter uzunluğunda olmalı");
            }


            /*
             
             PreviousStatus atama sırası önemli. 
            Geçmiş kaydı oluşturulurken PreviousStatus = fine.ApprovalStatus hâlâ eski durumu gösteriyor; 
            fine.ApprovalStatus = newStatus satırı ondan sonra geliyor. 
            Sırayı değiştirirseniz önceki durum ile yeni durum aynı yazılır.
             
             
             */
            trafficFine.ApprovalHistories.Add(new TrafficFineApprovalHistory
            {
                PerformedByUserId = userId,
                ActionDate = DateTime.Now,
                ActionType = actionType,
                Description = string.IsNullOrEmpty(description) ? null : description,
                PreviousStatus = trafficFine.ApprovalStatus,
                NewStatus = newStatus
            });

            trafficFine.ApprovalStatus = newStatus;
        }

        private async Task ValidateFineAsync(TrafficFine fine)
        {
            var fineType = await _trafficFineTypeDal.GetByIdAsync(fine.TrafficFineTypeId) ?? throw new BusinessException("Ceza Türü Bulunamadı");

            if(!fineType.IsActive)
            {
                throw new BusinessException("Pasif bir ceza türü seçilemez");
            }
            if(await _vehicleDal.GetByIdAsync(fine.VehicleId) is null)
            {
                throw new BusinessException("Araç bulunamadı");
            }
            if(fine.TrafficFineDate > DateTime.Now)
            {
                throw new BusinessException("İleri tarihte bir ceza tarihi olamaz");
            }
            if(fine.DateOfNotification is not null && fine.DateOfNotification.Value.Date < fine.TrafficFineDate)
            {
                throw new BusinessException("Tebliğ tarihi ceza tarihinden önce olamaz.");
            }

            var sameRecordNo = await _trafficFineDal.GetListAsync(f => f.TrafficFineRecordNo == fine.TrafficFineRecordNo && f.TrafficFineId != fine.TrafficFineId);

            if (sameRecordNo.Any())
            {
                throw new BusinessException("Bu tutanak numarasıyla kayıtlı bir ceza zaten var.");
            }
            
            // Tutar, kesildiği andaki güncel ceza türü tutarından alınır
            fine.TrafficFineAmount = fineType.Amount;
        }
    }
}
