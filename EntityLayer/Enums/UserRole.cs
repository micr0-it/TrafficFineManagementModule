using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Enums
{
    public enum UserRole
    {
        [Display(Name = "Admin")]
        Admin, //kullanıcı yönetimi
        [Display(Name = "Personel")]
        Personnel, //ceza girebilir, onaya gönderebilir
        [Display(Name = "Yönetici")]
        Manager, //yönetici onayı
        [Display(Name = "Finans")]
        Finance //finans onayı
    }
}
