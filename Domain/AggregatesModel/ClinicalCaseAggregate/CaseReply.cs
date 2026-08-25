using Domain.Core.Models;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AggregatesModel.ClinicalCaseAggregate
{
    public class CaseReply : Entity<Guid>
    {
        public Guid ClinicalCaseId { get; private set; }
        public Guid RequesterId { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private CaseReply() { }
        private CaseReply(Guid clinicalCaseId, Guid requesterId, string? content)
        {
            Id = Guid.NewGuid();
            ClinicalCaseId = clinicalCaseId;
            RequesterId = requesterId;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }
        public static CaseReply CreateCaseReply(Guid clinicalCaseId, Guid requesterId, string? content)
        {
            if (clinicalCaseId == Guid.Empty)
                throw new DomainException("O id do caso clínico não pode ser vazio.");

            if (requesterId == Guid.Empty)
                throw new DomainException("O id do solicitante não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(content))
                throw new DomainException("O conteúdo não pode ser vazio.");

            return new CaseReply(clinicalCaseId, requesterId, content);
        }
    }
}
