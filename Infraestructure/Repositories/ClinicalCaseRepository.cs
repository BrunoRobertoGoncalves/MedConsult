using Domain.AggregatesModel.ClinicalCaseAggregate.Repository;
using Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Repositories
{
    public class ClinicalCaseRepository : Repository<ClinicalCase, Guid>, IClinicalCaseRepository
    {
        public ClinicalCaseRepository(ApplicationDataContext context) : base(context)
        {
        }
    }
}
