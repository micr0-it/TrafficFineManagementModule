using EntityLayer.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class AppUser
    {
        [Key]
        public int AppUserId { get; set; }
        public string AppUserName { get; set; } = null!;
        public string PwdHash { get; set; } = null!;
        public string NameSurname { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.Personnel; //Herzaman yetki sistemlerinde en düşük yetki ile başlanır
        public bool IsActive { get; set; } = true;

        public ICollection<TrafficFine> EnteredFines{ get; set; } = new List<TrafficFine>(); //NullReferenceException’dan korunmak için
        public ICollection<TrafficFineApprovalHistory> PerformedActions { get; set; } = new List<TrafficFineApprovalHistory>();

    }
}
