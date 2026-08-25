using Application.DTOs;
using MediatR;

namespace Application.Queries.Users
{
    public class GetAllUsersQuery : IRequest<IReadOnlyCollection<UserDto>>
    {
        public GetAllUsersQuery(bool? activeOnly = null)
        {
            ActiveOnly = activeOnly;
        }

        public bool? ActiveOnly { get; private set; }
    }
}
