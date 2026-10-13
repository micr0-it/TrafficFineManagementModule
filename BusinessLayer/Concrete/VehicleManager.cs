using BusinessLayer.Abstract;
using BusinessLayer.Exceptions;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class VehicleManager : GenericManager<Vehicle>, IVehicleService
    {
        /// <summary>
        /// Türkiye plaka formatı: il kodu (01-81) + 1-3 harf + 2-4 rakam. Ör: 34ABC123
        /// </summary>
        
        private static readonly Regex PlatePattern = new(@"^(0[1-9]|[1-7][0-9]|8[01])[A-Z]{1,3}[0-9]{2,4}$", RegexOptions.Compiled);
        //private static readonly CultureInfo TurkishCulture = new("tr-TR");
        private readonly IVehicleDal _vehicleDal;

        public VehicleManager(IVehicleDal vehicleDal) : base(vehicleDal)
        {
            _vehicleDal = vehicleDal;
        }
        public override async Task AddAsync(Vehicle vehicle)
        {
            await PrepareAndValidateAsync(vehicle);
            await _vehicleDal.AddAsync(vehicle);
        }
        public override async Task UpdateAsync(Vehicle vehicle)
        {
            await PrepareAndValidateAsync(vehicle);
            await _vehicleDal.UpdateAsync(vehicle);
        }
        public Task<Vehicle?> GetByPlateAsync(string plate)
        {
            return _vehicleDal.GetByPlateAsync(NormalizePlate(plate));
        }

        /// <summary>
        /// "34 abc 123" ve "34ABC123" aynı plaka sayılsın diye boşluklar silinir, büyük harfe çevrilir.
        /// Plakalarda Türkçe karakter olmadığı için invariant kültür kullanılır.
        /// </summary>
        public static string NormalizePlate(string plate)
        {
           return plate.Replace(" ", "").ToUpperInvariant(); 
        }
        private async Task PrepareAndValidateAsync(Vehicle vehicle)
        {

            vehicle.VehiclePlate = NormalizePlate(vehicle.VehiclePlate);
            vehicle.VehicleBrand = vehicle.VehicleBrand.Trim();
            vehicle.VehicleModel = vehicle.VehicleModel.Trim();
            if (!PlatePattern.IsMatch(vehicle.VehiclePlate))
                throw new BusinessException("Geçersiz plaka formatı. Örnek: 34 ABC 123");
            //v.VehicleId != vehicle.VehicleId koşulu güncelleme için: araç düzenlenirken kendi plakasıyla çakışmasın.
            var existing = await _vehicleDal.GetListAsync(v =>
                v.VehiclePlate == vehicle.VehiclePlate && v.VehicleId != vehicle.VehicleId);

            if (existing.Any())
                throw new BusinessException("Bu plakayla kayıtlı bir araç zaten var.");
        }
    }
}
