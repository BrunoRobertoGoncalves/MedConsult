using Application.DTOs;

namespace Application.Queries.Users
{
    public interface IUserQueryService
    {
        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<UserDto>> GetAllAsync(bool? activeOnly, CancellationToken cancellationToken);
    }
}
