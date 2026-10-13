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
    public class DriverManager : GenericManager<Driver>, IDriverService
    {
        private readonly IDriverDal _driverDal;

        public DriverManager(IDriverDal driverDal) : base(driverDal)
        {
            _driverDal = driverDal;
        }
        public override async Task AddAsync(Driver driver)
        {
            await ValidateAsync(driver);
            await _driverDal.AddAsync(driver);
        }
        public override async Task UpdateAsync(Driver driver)
        {
            await ValidateAsync(driver);
            await _driverDal.UpdateAsync(driver);
        }

        private async Task ValidateAsync(Driver driver)
        {
            driver.NationIdentityNo = driver.NationIdentityNo.Trim();

            if (driver.NationIdentityNo.Length != 11 || !driver.NationIdentityNo.All(char.IsDigit))
                throw new BusinessException("TC kimlik numarası 11 haneli ve sadece rakamlardan oluşmalıdır.");

            var existing = await _driverDal.GetListAsync(d =>
                d.NationIdentityNo == driver.NationIdentityNo && d.DriverId != driver.DriverId);

            if (existing.Any())
                throw new BusinessException("Bu TC kimlik numarasıyla kayıtlı bir sürücü zaten var.");
        }
    }
}
