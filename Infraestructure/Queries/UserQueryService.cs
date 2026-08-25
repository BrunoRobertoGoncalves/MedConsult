using Application.DTOs;
using Application.Queries.Users;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Queries
{
    public class UserQueryService : IUserQueryService
    {
        private readonly ApplicationDataContext _dataContext;

        public UserQueryService(ApplicationDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dataContext.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(ProjectToDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<UserDto>> GetAllAsync(bool? activeOnly, CancellationToken cancellationToken)
        {
            var query = _dataContext.Users.AsNoTracking();

            if (activeOnly.HasValue)
                query = query.Where(u => u.IsActive == activeOnly.Value);

            return await query
                .Select(ProjectToDto())
                .ToListAsync(cancellationToken);
        }

        private static System.Linq.Expressions.Expression<Func<Domain.AggregatesModel.UserAggregate.User, UserDto>> ProjectToDto()
        {
            return u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email.Address,
                Crm = u.Crm.Number,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                Specialities = u.Specialities
            };
        }
    }
}
