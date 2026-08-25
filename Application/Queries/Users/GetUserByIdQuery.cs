using Application.DTOs;
using MediatR;

namespace Application.Queries.Users
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
