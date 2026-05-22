using Domain.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AggregatesModel.ExamAggregate
{
    public interface IExamRepository : IRepository<Exam, Guid>
    {
        void Delete(Guid examId);
    }
}
