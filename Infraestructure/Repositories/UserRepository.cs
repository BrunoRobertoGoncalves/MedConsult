using Domain.AggregatesModel.ClinicalCaseAggregate;
using Domain.AggregatesModel.UserAggregate;
using Domain.AggregatesModel.UserAggregate.Repository;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(ApplicationDataContext context) : base(context)
        {
        }
        public async Task<bool> ExistsByEmail(string email)
        {
            return await _entity.AnyAsync(u => u.Email.Address == email);
        }
        public async Task<bool> ExistsByCrm(string crm)
        {
            return await _entity.AnyAsync(u => u.Crm.Number == crm);
        }
    }
}
