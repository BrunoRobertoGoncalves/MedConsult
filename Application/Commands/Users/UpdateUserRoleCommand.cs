using Domain.Enums;
using MediatR;

namespace Application.Commands.Users
{
    public class UpdateUserRoleCommand : IRequest<bool>
    {
        public UpdateUserRoleCommand(Guid id, UserRole newRole)
        {
            Id = id;
            NewRole = newRole;
        }

        public Guid Id { get; private set; }

        public UserRole NewRole { get; private set; }
    }
}
