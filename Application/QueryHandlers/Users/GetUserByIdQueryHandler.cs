using Application.DTOs;
using Application.Queries.Users;
using MediatR;

namespace Application.QueryHandlers.Users
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserQueryService _userQueryService;

        public GetUserByIdQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return _userQueryService.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
