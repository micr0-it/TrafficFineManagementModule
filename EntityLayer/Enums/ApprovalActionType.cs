using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Enums
{
    /// <summary>
    /// Onay geçmişindeki işlem tipi.
    /// </summary>
    public enum ApprovalActionType
    {
        [Display(Name = "Oluşturuldu")]
        Created,
        [Display(Name = "Onaya Gönderildi")]
        Submitted,
        [Display(Name = "Onaylandı")]
        Approved,
        [Display(Name = "Reddedildi")]
        Rejected
    }
}
