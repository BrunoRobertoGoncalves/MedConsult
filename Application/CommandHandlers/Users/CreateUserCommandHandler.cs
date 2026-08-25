using Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Exceptions;
using MediatR;

namespace Application.CommandHandlers.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (await _userRepository.ExistsByEmail(request.Email))
                throw new DomainException($"O e-mail '{request.Email}' já está cadastrado.");

            if (await _userRepository.ExistsByCrm(request.Crm))
                throw new DomainException($"O CRM '{request.Crm}' já está cadastrado.");

            var user = User.CreateUser(
                request.Name,
                request.Email,
                request.Crm,
                request.PasswordHash,
                request.Role,
                request.Specialities);

            await _userRepository.Add(user);

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return user.Id;
        }
    }
}
