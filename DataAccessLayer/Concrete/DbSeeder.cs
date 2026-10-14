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
        private const string TestPassword = "Test123";

        public static async Task SeedAsync(AppDbContext db)
        {
            await SeedUsersAsync(db);
            await SeedTrafficFineTypesAsync(db);
            await SeedVehiclesAsync(db);
            await SeedDriversAsync(db);
            await SeedTrafficFinesAsync(db);
        }

        // ---------- Kullanıcılar ----------

        private static async Task SeedUsersAsync(AppDbContext db)
        {
            var hasher = new PasswordHasher<AppUser>();

            var seedUsers = new List<AppUser>
            {
                new() { AppUserName = "admin",    NameSurname = "Sistem Yöneticisi", RegistrationNumber = "0001", Role = UserRole.Admin },
                new() { AppUserName = "personel", NameSurname = "Ahmet Kaya",        RegistrationNumber = "0002", Role = UserRole.Personnel },
                new() { AppUserName = "yonetici", NameSurname = "Elif Demir",        RegistrationNumber = "0003", Role = UserRole.Manager },
                new() { AppUserName = "finans",   NameSurname = "Murat Şahin",       RegistrationNumber = "0004", Role = UserRole.Finance }
            };

            foreach (var appUser in seedUsers)
            {
                bool exists = await db.AppUsers.AnyAsync(u => u.AppUserName == appUser.AppUserName);
                if (exists)
                {
                    continue;
                }

                appUser.PwdHash = hasher.HashPassword(appUser, TestPassword);
                db.AppUsers.Add(appUser);
            }

            await db.SaveChangesAsync();
        }

        // ---------- Ceza türleri ----------

        private static async Task SeedTrafficFineTypesAsync(AppDbContext db)
        {
            if (await db.TrafficFineTypes.AnyAsync())
            {
                return;
            }

            // Tutarlar ve puanlar örnektir, güncel resmi tutarları yansıtmaz.
            db.TrafficFineTypes.AddRange(
                new TrafficFineType { Article = "KTK 51/2-a", Description = "Hız sınırını %10-30 aşmak", Amount = 2000m, TrafficFinePoint = 10 },
                new TrafficFineType { Article = "KTK 51/2-b", Description = "Hız sınırını %30-50 aşmak", Amount = 4000m, TrafficFinePoint = 15 },
                new TrafficFineType { Article = "KTK 47/1-b", Description = "Kırmızı ışıkta geçmek", Amount = 2500m, TrafficFinePoint = 20 },
                new TrafficFineType { Article = "KTK 65/1-a", Description = "Azami yüklü ağırlığı aşmak", Amount = 6000m, TrafficFinePoint = 15 },
                new TrafficFineType { Article = "KTK 78/1-a", Description = "Emniyet kemeri takmamak", Amount = 1000m, TrafficFinePoint = 5 },
                new TrafficFineType { Article = "KTK 61/1-a", Description = "Yasak yere park etmek", Amount = 900m, TrafficFinePoint = 0 },
                new TrafficFineType { Article = "KTK 35/1", Description = "Seyir defteri bulundurmamak (yürürlükten kalkmış örnek)", Amount = 500m, TrafficFinePoint = 0, IsActive = false }
            );

            await db.SaveChangesAsync();
        }

        // ---------- Araçlar ----------

        private static async Task SeedVehiclesAsync(AppDbContext db)
        {
            if (await db.Vehicles.AnyAsync())
            {
                return;
            }

            db.Vehicles.AddRange(
                new Vehicle { VehiclePlate = "34ABC123", VehicleType = VehicleType.Passenger, VehicleBrand = "Renault", VehicleModel = "Clio", VehicleRegistrationDate = new DateTime(2022, 3, 15) },
                new Vehicle { VehiclePlate = "41BN78", VehicleType = VehicleType.Passenger, VehicleBrand = "Toyota", VehicleModel = "Corolla", VehicleRegistrationDate = new DateTime(2023, 6, 1) },
                new Vehicle { VehiclePlate = "34TR4567", VehicleType = VehicleType.Tractor, VehicleBrand = "Mercedes-Benz", VehicleModel = "Actros", VehicleLicenceSerialNo = "AB123456", VehicleRegistrationDate = new DateTime(2021, 9, 20) },
                new Vehicle { VehiclePlate = "35XY456", VehicleType = VehicleType.Tractor, VehicleBrand = "Volvo", VehicleModel = "FH 500", VehicleLicenceSerialNo = "AC654321", VehicleRegistrationDate = new DateTime(2020, 11, 5) },
                new Vehicle { VehiclePlate = "34DR890", VehicleType = VehicleType.Trailer, VehicleBrand = "Tırsan", VehicleModel = "Tenteli Dorse", VehicleRegistrationDate = new DateTime(2021, 9, 20) },
                new Vehicle { VehiclePlate = "06KRL123", VehicleType = VehicleType.Rental, VehicleBrand = "Fiat", VehicleModel = "Egea" }
            );

            await db.SaveChangesAsync();
        }

        // ---------- Sürücüler ----------

        private static async Task SeedDriversAsync(AppDbContext db)
        {
            if (await db.Drivers.AnyAsync())
            {
                return;
            }

            // TC kimlik numaraları örnek amaçlıdır, gerçek kişilere ait değildir.
            db.Drivers.AddRange(
                new Driver { DriverNameSurname = "Hasan Yıldız", NationIdentityNo = "10000000001", DriverLicenceNo = "TR100001", DriverLicenceClass = "CE" },
                new Driver { DriverNameSurname = "Kemal Arslan", NationIdentityNo = "10000000002", DriverLicenceNo = "TR100002", DriverLicenceClass = "CE" },
                new Driver { DriverNameSurname = "Zeynep Çelik", NationIdentityNo = "10000000003", DriverLicenceNo = "TR100003", DriverLicenceClass = "B" },
                new Driver { DriverNameSurname = "Burak Öztürk", NationIdentityNo = "10000000004", DriverLicenceNo = "TR100004", DriverLicenceClass = "B" }
            );

            await db.SaveChangesAsync();
        }

        // ---------- Örnek cezalar (her aşamadan) ----------

        private static async Task SeedTrafficFinesAsync(AppDbContext db)
        {
            if (await db.TrafficFines.AnyAsync())
            {
                return;
            }

            var personnel = await db.AppUsers.FirstAsync(u => u.AppUserName == "personel");
            var manager = await db.AppUsers.FirstAsync(u => u.AppUserName == "yonetici");
            var finance = await db.AppUsers.FirstAsync(u => u.AppUserName == "finans");

            var fineTypes = await db.TrafficFineTypes.ToDictionaryAsync(t => t.Article);
            var vehicles = await db.Vehicles.ToDictionaryAsync(v => v.VehiclePlate);
            var drivers = await db.Drivers.ToDictionaryAsync(d => d.NationIdentityNo);

            var now = DateTime.Now;

            // 1) Yeni: personel girdi, henüz onaya göndermedi
            var newFine = CreateFine("2026-0001", now.AddDays(-1), fineTypes["KTK 61/1-a"], vehicles["06KRL123"], drivers["10000000004"], personnel);
            AddCreated(newFine, personnel, now.AddDays(-1).AddHours(2));

            // 2) Yönetici onayında
            var pendingManagerFine = CreateFine("2026-0002", now.AddDays(-3), fineTypes["KTK 51/2-a"], vehicles["34ABC123"], drivers["10000000003"], personnel);
            AddCreated(pendingManagerFine, personnel, now.AddDays(-3).AddHours(3));
            AddSubmitted(pendingManagerFine, personnel, now.AddDays(-3).AddHours(4));

            // 3) Finans onayında
            var pendingFinanceFine = CreateFine("2026-0003", now.AddDays(-6), fineTypes["KTK 65/1-a"], vehicles["34TR4567"], drivers["10000000001"], personnel);
            AddCreated(pendingFinanceFine, personnel, now.AddDays(-6).AddHours(2));
            AddSubmitted(pendingFinanceFine, personnel, now.AddDays(-6).AddHours(3));
            AddApproved(pendingFinanceFine, manager, now.AddDays(-5), "Tartı belgesi kontrol edildi, ceza yerinde.");

            // 4) Tamamlandı (sürücüsü bilinmeyen EDS cezası)
            var completedFine = CreateFine("2026-0004", now.AddDays(-12), fineTypes["KTK 51/2-b"], vehicles["35XY456"], null, personnel);
            AddCreated(completedFine, personnel, now.AddDays(-12).AddHours(5));
            AddSubmitted(completedFine, personnel, now.AddDays(-12).AddHours(6));
            AddApproved(completedFine, manager, now.AddDays(-11), "EDS kaydı incelendi.");
            AddApproved(completedFine, finance, now.AddDays(-10), "İndirimli süre içinde ödendi.");

            // 5) Tamamlandı (sürücüsü belli)
            var completedFine2 = CreateFine("2026-0005", now.AddDays(-15), fineTypes["KTK 78/1-a"], vehicles["41BN78"], drivers["10000000003"], personnel);
            AddCreated(completedFine2, personnel, now.AddDays(-15).AddHours(1));
            AddSubmitted(completedFine2, personnel, now.AddDays(-15).AddHours(2));
            AddApproved(completedFine2, manager, now.AddDays(-14), null);
            AddApproved(completedFine2, finance, now.AddDays(-13), null);

            // 6) Yönetici tarafından reddedildi
            var rejectedByManagerFine = CreateFine("2026-0006", now.AddDays(-9), fineTypes["KTK 47/1-b"], vehicles["34DR890"], drivers["10000000002"], personnel);
            AddCreated(rejectedByManagerFine, personnel, now.AddDays(-9).AddHours(2));
            AddSubmitted(rejectedByManagerFine, personnel, now.AddDays(-9).AddHours(3));
            AddRejected(rejectedByManagerFine, manager, now.AddDays(-8),
                "Dorse tek başına hareket edemez; ceza çekici plakasına kesilmeliydi. Tutanak iptali için itiraz edilecek.");

            // 7) Finans tarafından reddedildi
            var rejectedByFinanceFine = CreateFine("2026-0007", now.AddDays(-20), fineTypes["KTK 51/2-a"], vehicles["06KRL123"], drivers["10000000004"], personnel);
            AddCreated(rejectedByFinanceFine, personnel, now.AddDays(-20).AddHours(1));
            AddSubmitted(rejectedByFinanceFine, personnel, now.AddDays(-20).AddHours(2));
            AddApproved(rejectedByFinanceFine, manager, now.AddDays(-19), "Sürücüye bilgi verildi.");
            AddRejected(rejectedByFinanceFine, finance, now.AddDays(-18),
                "Kiralık araç; sözleşme gereği ceza kiralama şirketine yansıtılacak, şirket bütçesinden ödenmeyecek.");

            db.TrafficFines.AddRange(
                newFine, pendingManagerFine, pendingFinanceFine,
                completedFine, completedFine2,
                rejectedByManagerFine, rejectedByFinanceFine);

            await db.SaveChangesAsync();
        }


        // --- Örnek ceza oluşturmak için yardımcı metotlar ---
        private static TrafficFine CreateFine(string recordNo, DateTime fineDate, TrafficFineType trafficFineType, Vehicle vehicle, Driver? driver, AppUser recordedBy)
        {
            return new TrafficFine
            {
                TrafficFineRecordNo = recordNo,
                TrafficFineDate = fineDate,
                DateOfNotification = fineDate.Date.AddDays(1),
                TrafficFineType = trafficFineType,
                TrafficFineAmount = trafficFineType.Amount,
                Vehicle = vehicle,
                Driver = driver,
                RecordedByPersonnel = recordedBy,
                ApprovalStatus = ApprovalStatus.New
            };
        }

        private static void AddCreated(TrafficFine trafficFine, AppUser user, DateTime dateTime)
        {
            AddHistory(trafficFine, user, dateTime, ApprovalActionType.Created, null, ApprovalStatus.New, null);
        }
        private static void AddSubmitted(TrafficFine trafficFine, AppUser user, DateTime date)
        {
            AddHistory(trafficFine, user, date, ApprovalActionType.Submitted,
                trafficFine.ApprovalStatus, ApprovalStatus.PendingManagerApproval, null);
        }

        /// <summary>
        /// Onay akışına göre bir sonraki aşamaya geçirir: Yönetici → Finans → Tamamlandı.
        /// </summary>
        private static void AddApproved(TrafficFine trafficFine, AppUser user, DateTime date, string? description)
        {
            var nextStatus = trafficFine.ApprovalStatus == ApprovalStatus.PendingManagerApproval
                ? ApprovalStatus.PendingFinanceApproval
                : ApprovalStatus.Completed;

            AddHistory(trafficFine, user, date, ApprovalActionType.Approved,
                trafficFine.ApprovalStatus, nextStatus, description);
        }

        private static void AddRejected(TrafficFine trafficFine, AppUser user, DateTime date, string reason)
        {
            AddHistory(trafficFine, user, date, ApprovalActionType.Rejected,
                trafficFine.ApprovalStatus, ApprovalStatus.Rejected, reason);
        }

        /// <summary>
        /// Geçmiş kaydını ekler ve cezanın durumunu günceller (TrafficFineManager.AddTransition ile aynı mantık).
        /// </summary>
        private static void AddHistory(TrafficFine trafficFine, AppUser user, DateTime date,
            ApprovalActionType actionType, ApprovalStatus? previousStatus, ApprovalStatus newStatus, string? description)
        {
            trafficFine.ApprovalHistories.Add(new TrafficFineApprovalHistory
            {
                PerformedByUser = user,
                ActionDate = date,
                ActionType = actionType,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                Description = description
            });

            trafficFine.ApprovalStatus = newStatus;
        }
    }
}
