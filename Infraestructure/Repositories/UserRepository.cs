using Domain.AggregatesModel.ClinicalCaseAggregate;
using Domain.AggregatesModel.UserAggregate;
using Domain.AggregatesModel.UserAggregate.Repository;
using Infraestructure.Data;

namespace Infraestructure.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(ApplicationDataContext context) : base(context)
        {
        }
    }
}
