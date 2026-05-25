using Domain.Core.Models;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.AggregatesModel.ClinicalCaseAggregate
{
    public class CaseStatusHistory : Entity<Guid>
    {
        public Guid ClinicalCaseId { get; private set; }
        public CaseStatus? FromStatus { get; private set; }
        public CaseStatus ToStatus { get; private set; }
        public Guid ChangedByUserId { get; private set; }
        public DateTime ChangedAt { get; private set; }

        private CaseStatusHistory() { }
        private CaseStatusHistory(Guid clinicalCaseId, CaseStatus? fromStatus, CaseStatus toStatus, Guid changedByUserId)
        {
            Id = Guid.NewGuid();
            ClinicalCaseId = clinicalCaseId;
            FromStatus = fromStatus;
            ToStatus = toStatus;
            ChangedByUserId = changedByUserId;
            ChangedAt = DateTime.UtcNow;
        }

        public static CaseStatusHistory CreateCaseStatusHistory(Guid clinicalCaseId, CaseStatus? fromStatus, CaseStatus toStatus, Guid changedByUserId)
        {
            if (clinicalCaseId == Guid.Empty)
                throw new DomainException("ClinicalCaseId cannot be empty.");
            if (fromStatus.HasValue && !Enum.IsDefined(typeof(CaseStatus), fromStatus.Value))
                throw new DomainException("Invalid FromStatus value.");
            if (!Enum.IsDefined(typeof(CaseStatus), toStatus))
                throw new DomainException("Invalid ToStatus value.");
            if (changedByUserId == Guid.Empty)
                throw new DomainException("ChangedByUserId cannot be empty.");

            return new CaseStatusHistory(clinicalCaseId, fromStatus, toStatus, changedByUserId);
        }
    }
}
