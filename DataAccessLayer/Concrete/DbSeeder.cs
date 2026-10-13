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
            if (await db.AppUsers.AnyAsync())
                return;

            //var admin = new AppUser
            //{
            //    AppUserName = "admin",
            //    NameSurname = "Sistem Yöneticisi",
            //    RegistrationNumber = "0001",
            //    Role = UserRole.Admin
            //};

            //admin.PwdHash = new PasswordHasher<AppUser>().HashPassword(admin, "Admin123");

            //db.AppUsers.Add(admin);

            var hasher = new PasswordHasher<AppUser>();
            var users = new List<AppUser>{
                new() { AppUserName = "admin", NameSurname ="Sistem Yöneticisi", RegistrationNumber = "0001",Role = UserRole.Admin},
                new() { AppUserName = "personel", NameSurname ="Test Personel", RegistrationNumber = "0002",Role = UserRole.Personnel},
                new() { AppUserName = "yonetici", NameSurname ="Test Yönetici", RegistrationNumber = "0003",Role = UserRole.Manager},
                new() { AppUserName = "finans", NameSurname ="Test Finans", RegistrationNumber = "0004",Role = UserRole.Finance}
            };

            foreach (var user in users)
            {
                user.PwdHash = hasher.HashPassword(user, "Test123");
            }
            db.AppUsers.AddRange(users);
            await db.SaveChangesAsync();
        }
    }
}
