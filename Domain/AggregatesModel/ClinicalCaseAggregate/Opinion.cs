using Domain.Core.Models;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.AggregatesModel.ClinicalCaseAggregate
{
    public class Opinion : Entity<Guid>
    {
        public Guid ClinicalCaseId { get; private set; }
        public Guid SpecialistId { get; private set; }
        public string Content { get; private set; }
        public OpinionAgreement Agreement { get; private set; }
        public string ConductRecommendation { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Opinion() { }

        private Opinion(Guid clinicalCaseId, Guid specialistId, string content, OpinionAgreement agreement, string conductRecommendation)
        {
            Id = Guid.NewGuid();
            ClinicalCaseId = clinicalCaseId;
            SpecialistId = specialistId;
            Content = content;
            Agreement = agreement;
            CreatedAt = DateTime.UtcNow;
            ConductRecommendation = conductRecommendation;
        }

        public static Opinion CreateOpinion(Guid clinicalCaseId, Guid specialistId, string content, OpinionAgreement agreement, string conductRecommendation)
        {
            if (clinicalCaseId == Guid.Empty)
                throw new DomainException("ClinicalCaseId cannot be empty.");
            if (specialistId == Guid.Empty)
                throw new DomainException("specialistId not found.");
            if (string.IsNullOrWhiteSpace(content))
                throw new DomainException("Content cannot be empty.");
            if (!Enum.IsDefined(typeof(OpinionAgreement), agreement))
                throw new DomainException("Invalid OpinionAgreement value.");
            if(string.IsNullOrWhiteSpace(conductRecommendation))
                throw new DomainException("ConductRecommendation cannot be empty.");

            return new Opinion(clinicalCaseId, specialistId, content, agreement, conductRecommendation);
        }
    }
}
