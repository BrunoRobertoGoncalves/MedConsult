using Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Exceptions;
using MediatR;

namespace Application.CommandHandlers.Users
{
    public class DisableUserCommandHandler : IRequestHandler<DisableUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        public DisableUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(DisableUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null) throw new DomainException("User not found");

            user.Deactivate();

            var entitySaved = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

            if (!entitySaved)
                return false;

            return true;
        }
    }
}
