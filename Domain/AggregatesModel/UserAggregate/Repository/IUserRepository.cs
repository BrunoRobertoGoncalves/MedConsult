using Domain.Core.Data;

namespace Domain.AggregatesModel.UserAggregate.Repository
{
    internal interface IUserRepository : IRepository<User, Guid>
    {
    }
}
