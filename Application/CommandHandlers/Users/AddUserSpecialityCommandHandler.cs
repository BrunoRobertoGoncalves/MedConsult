using Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Exceptions;
using MediatR;

namespace Application.CommandHandlers.Users
{
    public class AddUserSpecialityCommandHandler : IRequestHandler<AddUserSpecialityCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public AddUserSpecialityCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(AddUserSpecialityCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new DomainException("User not found");

            user.AddSpeciality(request.Speciality);

            var entitySaved = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

            if (!entitySaved)
                return false;

            return true;
        }
    }
}
