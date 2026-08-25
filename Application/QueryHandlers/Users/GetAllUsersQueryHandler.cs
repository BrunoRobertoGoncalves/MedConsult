using Application.DTOs;
using Application.Queries.Users;
using MediatR;

namespace Application.QueryHandlers.Users
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyCollection<UserDto>>
    {
        private readonly IUserQueryService _userQueryService;

        public GetAllUsersQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public Task<IReadOnlyCollection<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return _userQueryService.GetAllAsync(request.ActiveOnly, cancellationToken);
        }
    }
}
