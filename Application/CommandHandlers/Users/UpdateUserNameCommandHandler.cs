using Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Exceptions;
using MediatR;

namespace Application.CommandHandlers.Users
{
    public class UpdateUserNameCommandHandler : IRequestHandler<UpdateUserNameCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserNameCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new DomainException("User not found");

            user.UpdateName(request.NewUserName);

            var entitySaved = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

            if (!entitySaved)
                return false;

            return true;
        }
    }
}
