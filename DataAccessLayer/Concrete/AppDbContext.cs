using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<TrafficFineType> TrafficFineTypes { get; set; }
        public DbSet<TrafficFine> TrafficFines { get; set; }
        public DbSet<TrafficFineApprovalHistory> TrafficFineApprovalHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>(e =>
            {
                e.HasIndex(u => u.AppUserName).IsUnique();
                e.Property(u => u.Role).HasConversion<string>();
            });

            builder.Entity<Driver>().HasIndex(d => d.NationIdentityNo).IsUnique();

            builder.Entity<Vehicle>(e =>
            {
                e.HasIndex(v => v.VehiclePlate).IsUnique();
                e.Property(v => v.VehicleType).HasConversion<string>();
            });


            builder.Entity<TrafficFineType>()
                .Property(t => t.Amount).HasPrecision(18, 2);

            builder.Entity<TrafficFine>(e =>
            {
                e.HasIndex(f => f.TrafficFineRecordNo).IsUnique();
                e.Property(f => f.TrafficFineAmount).HasPrecision(18, 2);
                e.Property(f => f.ApprovalStatus).HasConversion<string>();

                e.HasOne(f => f.TrafficFineType)
                 .WithMany(t => t.Fines)
                 .HasForeignKey(f => f.TrafficFineTypeId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(f => f.Vehicle)
                 .WithMany(v => v.VehicleFines)
                 .HasForeignKey(f => f.VehicleId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(f => f.Driver)
                 .WithMany(d => d.TrafficFines)
                 .HasForeignKey(f => f.DriverId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(f => f.RecordedByPersonnel)
                 .WithMany(u => u.EnteredFines)
                 .HasForeignKey(f => f.RecordedByPersonnelId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<TrafficFineApprovalHistory>(e => {
                e.Property(h => h.ActionType).HasConversion<string>();
                e.Property(h => h.PreviousStatus).HasConversion<string>();
                e.Property(h => h.NewStatus).HasConversion<string>();
                e.Property(h => h.Description).HasMaxLength(500);


                ///<summary>
                ///Geçmiş -> ceza ilişkisinde bilinçli olarak Cascade kullandım
                ///sadece “Yeni” durumdaki (henüz onaya gönderilmemiş) cezalar silinebilecek, 
                ///onların geçmişinde de yalnızca “Oluşturuldu” kaydı olur
                ///ceza silinince bu kaydın kalmasının anlamı yok.
                ///Onaya girmiş cezaların silinmesini Business katmanında engelleyeceğim.
                /// </summary>
                e.HasOne(h => h.TrafficFine)
                 .WithMany(f => f.ApprovalHistories)
                 .HasForeignKey(h => h.TrafficFineId)
                 .OnDelete(DeleteBehavior.Cascade);
                
                e.HasOne(h => h.PerformedByUser)
                 .WithMany(f => f.PerformedActions)
                 .HasForeignKey(h => h.PerformedByUserId)
                 .OnDelete(DeleteBehavior.Restrict);

                
            });
        }
    }
}