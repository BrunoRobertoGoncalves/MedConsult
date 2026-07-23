using Domain.Core.Data;

namespace Domain.AggregatesModel.UserAggregate.Repository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<bool> ExistsByEmail(string email);
        Task<bool> ExistsByCrm(string crm);
    }
}
