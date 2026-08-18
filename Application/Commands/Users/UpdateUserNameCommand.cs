using MediatR;

namespace Application.Commands.Users
{
    public class UpdateUserNameCommand : IRequest<bool>
    {
        public UpdateUserNameCommand(Guid id, string newUserName)
        {
            Id = id;
            NewUserName = newUserName;
        }

        public Guid Id {  get; private set; }
        public string NewUserName { get; private set; }
    }
}
