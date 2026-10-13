using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Enums
{
    public enum ApprovalStatus
    {
        /// <summary>
        /// cezanın sisteme kaydının admin tarafından onaylanmasıyla ilgili.
        /// </summary>
        PendingManagerApproval,
        PendingFinanceApproval,
        Completed,
        Rejected,
        New // Personel girdi, henüz onaya göndermedi
    }
}
