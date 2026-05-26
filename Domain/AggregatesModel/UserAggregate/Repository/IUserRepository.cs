using Domain.Core.Data;

namespace Domain.AggregatesModel.UserAggregate.Repository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
