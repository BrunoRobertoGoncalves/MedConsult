using Domain.Core.Models;
using Domain.Exceptions;

namespace Domain.AggregatesModel.ClinicalCaseAggregate
{
    public class CaseExam : Entity<Guid>
    {
        public Guid ClinicalCaseId { get; private set; }
        public Guid ExamId { get; private set; }
        public DateTime LinkedAt { get; private set; }

        private CaseExam() { }

        private CaseExam(Guid clinicalCaseId, Guid examId)
        {
            Id = Guid.NewGuid();
            ClinicalCaseId = clinicalCaseId;
            ExamId = examId;
            LinkedAt = DateTime.UtcNow;
        }

        public static CaseExam CreateCaseExam(Guid clinicalCaseId, Guid examId)
        {
            if (clinicalCaseId == Guid.Empty)
                throw new DomainException("ClinicalCase id cannot be null");

            if (examId == Guid.Empty)
                throw new DomainException("Exam id cannot be null");

            return new CaseExam(clinicalCaseId, examId);
        }
    }
}
