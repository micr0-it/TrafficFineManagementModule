using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Workflow
{
    /// <summary>
    /// Onay akışındaki tek bir aşamanın tanımı.
    /// </summary>
    /// <param name="CurrentStatus">Bu aşamanın durumu</param>
    /// <param name="ResponsibleRole">Bu aşamada işlem yapabilecek rol</param>
    /// <param name="NextStatus">Onaylanınca geçilecek durum</param>
    /// <param name="ApproveActionType">Onay işleminin geçmişe yazılacak tipi</param>
    /// <param name="CanBeRejected">Bu aşamada ret yapılabilir mi</param>

    //record, sadece veri taşıyan değişmez (immutable) sınıflar için C#’ın kısa yazımı.
    //Tek satırda constructor ve property’leri birlikte tanımlar.
    public sealed record ApprovalStep(
        ApprovalStatus CurrentStatus,
        UserRole ResponsibleRole,
        ApprovalStatus NextStatus,
        ApprovalActionType ApproveActionType,
        bool CanBeRejected
    );
}
