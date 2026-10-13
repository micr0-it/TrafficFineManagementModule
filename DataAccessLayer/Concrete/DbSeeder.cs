using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Identity;      // PasswordHasher için
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (await db.AppUsers.AnyAsync(k => k.Role == UserRole.Admin))
                return;

            var admin = new AppUser
            {
                AppUserName = "admin",
                NameSurname = "Sistem Yöneticisi",
                RegistrationNumber = "0001",
                Role = UserRole.Admin
            };
            admin.PwdHash = new PasswordHasher<AppUser>().HashPassword(admin, "Admin123");

            db.AppUsers.Add(admin);
            await db.SaveChangesAsync();
        }
    }
}
