using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Enums
{
    public enum UserRole
    {
        Admin, //kullanıcı yönetimi
        Personnel, //ceza girebilir, onaya gönderebilir
        Manager, //yönetici onayı
        Finance //finans onayı
    }
}
