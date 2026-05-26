using Domain.Core.Data;

namespace Domain.AggregatesModel.ExamAggregate.Repository
{
    public interface IExamRepository : IRepository<Exam, Guid>
    {
        void Delete(Exam exam);
    }
}
