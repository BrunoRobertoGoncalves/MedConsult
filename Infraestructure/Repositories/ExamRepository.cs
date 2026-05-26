using Domain.AggregatesModel.ExamAggregate;
using Domain.AggregatesModel.ExamAggregate.Repository;
using Infraestructure.Data;

namespace Infraestructure.Repositories
{
    public class ExamRepository : Repository<Exam, Guid>, IExamRepository
    {
        public ExamRepository(ApplicationDataContext context) : base(context)
        {
        }

        public void Delete(Exam exam)
        {
            _entity.Remove(exam);
        }
    }
}
