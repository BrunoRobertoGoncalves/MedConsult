using MediatR;

namespace API.Application.Commands.Users
{
    public class EnableUserCommand : IRequest<bool>
    {
        public EnableUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
