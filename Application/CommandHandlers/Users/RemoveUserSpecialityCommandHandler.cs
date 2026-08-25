using Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Exceptions;
using MediatR;

namespace Application.CommandHandlers.Users
{
    public class RemoveUserSpecialityCommandHandler : IRequestHandler<RemoveUserSpecialityCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserSpecialityCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(RemoveUserSpecialityCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new DomainException("Usuário não encontrado.");

            user.RemoveSpeciality(request.Speciality);

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}
