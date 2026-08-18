using Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Exceptions;
using MediatR;

namespace Application.CommandHandlers.Users
{
    internal class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserEmailCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new DomainException("User not found");

            user.UpdateEmail(request.Email);

            var entitySaved = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

            if (!entitySaved)
                return false;

            return true;
        }
    }
}
