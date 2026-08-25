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
                throw new DomainException("O id do caso clínico não pode ser vazio.");
            if (specialistId == Guid.Empty)
                throw new DomainException("O id do especialista não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(content))
                throw new DomainException("O conteúdo não pode ser vazio.");
            if (!Enum.IsDefined(typeof(OpinionAgreement), agreement))
                throw new DomainException("Valor de concordância do parecer inválido.");
            if(string.IsNullOrWhiteSpace(conductRecommendation))
                throw new DomainException("A recomendação de conduta não pode ser vazia.");

            return new Opinion(clinicalCaseId, specialistId, content, agreement, conductRecommendation);
        }
    }
}
