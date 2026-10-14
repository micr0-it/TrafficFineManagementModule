using BusinessLayer.Abstract;
using BusinessLayer.Exceptions;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AppUserManager : GenericManager<AppUser>, IAppUserService
    {
        private const int MinPwdLength = 10;
        private readonly IAppUserDal _appUserDal;
        private readonly PasswordHasher<AppUser> _hasher = new();
        public AppUserManager(IAppUserDal userDal, IAppUserDal appUserDal) : base(userDal)
        {
            _appUserDal = appUserDal;
        }


        /// <summary>
        /// Admin tarafından personel, yönetici veya finans kullanıcısı oluşturulur.
        /// </summary>
        public async Task CreateUserAsync(AppUser user, string pwd)
        {
            if(user.Role == UserRole.Admin)
            {
                throw new BusinessException("Bu ekrandan admin kullanıcısı oluşturulamaz.");

            }
            if (string.IsNullOrWhiteSpace(pwd) || pwd.Length < MinPwdLength)
            {
                throw new BusinessException($"Şifre en az {MinPwdLength} karakter olmalıdır.");
            }

            user.AppUserName = user.AppUserName.Trim();
            
            if(await _appUserDal.GetByUserNameAsync(user.AppUserName) is not null)
            {
                throw new BusinessException("Bu kullanıcı adı zaten kullanılıyor.");
            }

            user.IsActive = true;
            user.PwdHash = _hasher.HashPassword(user, pwd);

            await _appUserDal.AddAsync(user);
        }
        public Task AddAsync(AppUser t)
        {
            throw new NotImplementedException();
        }

        

        public Task DeleteAsync(AppUser t)
        {
            throw new BusinessException("Kullanıcılar silinemez, pasif yapılabilir.");
        }

       

        public async Task SetActiveStatusAsync(int userId, bool isActive)
        {
            var user = await _appUserDal.GetByIdAsync(userId) ?? throw new BusinessException("Kullanıcı bulunamadı.");
            
            if(user.Role == UserRole.Admin && !isActive)
            {
                throw new BusinessException("Admin kullanıcısı pasif yapılamaz.");
            }
            
            user.IsActive = isActive;
            await _appUserDal.UpdateAsync(user);
        }

        public Task UpdateAsync(AppUser t)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Bilgiler doğruysa kullanıcıyı, değilse null döner.
        /// Kullanıcı adının mı şifrenin mi yanlış olduğu bilinçli olarak ayırt edilmez.
        /// </summary>
        public async Task<AppUser?> ValidateLoginAsync(string userName, string pwd)
        {
            if (string.IsNullOrWhiteSpace(userName)|| string.IsNullOrWhiteSpace(pwd))
            {
                return null;
            }

            var user = await _appUserDal.GetByUserNameAsync(userName.Trim());
            if (user is null || !user.IsActive)
            {
                return null;
            }
            var result = _hasher.VerifyHashedPassword(user, user.PwdHash, pwd);
            return result == PasswordVerificationResult.Failed ? null : user;
        }

        
    }
}
