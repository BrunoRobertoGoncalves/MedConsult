using Domain.Core.Data;

namespace Domain.AggregatesModel.UserAggregate
{
    internal interface IUserRepository : IRepository<User, Guid>
    {
    }
}
