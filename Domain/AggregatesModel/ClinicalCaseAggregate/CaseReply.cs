using Domain.Core.Models;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AggregatesModel.ClinicalCaseAggregate
{
    public class CaseReply : Entity<Guid>, IAggregateRoot
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
                throw new DomainException("Clinical case id cannot be empty");

            if (requesterId == Guid.Empty)
                throw new DomainException("Requester id cannot be empty");

            if (string.IsNullOrWhiteSpace(content))
                throw new DomainException("Content cannot be empty");

            return new CaseReply(clinicalCaseId, requesterId, content);
        }
    }
}
