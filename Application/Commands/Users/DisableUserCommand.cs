using MediatR;

namespace Application.Commands.Users
{
    public class DisableUserCommand : IRequest<bool>
    {
        public DisableUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
