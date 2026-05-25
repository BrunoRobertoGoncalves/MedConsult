using Domain.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AggregatesModel.ClinicalCaseAggregate.Repository
{
    internal interface IClinicalCaseRepository : IRepository<ClinicalCase, Guid>
    {
    }
}
