using Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Exceptions;
using MediatR;

namespace Application.CommandHandlers.Users
{
    public class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserEmailCommand, bool>
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
                throw new DomainException("Usuário não encontrado.");

            if (await _userRepository.ExistsByEmail(request.Email))
                throw new DomainException($"O e-mail '{request.Email}' já está cadastrado.");

            user.UpdateEmail(request.Email);

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}
