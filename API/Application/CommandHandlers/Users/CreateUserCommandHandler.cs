using API.Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Exceptions;
using Infraestructure.Repositories;
using MediatR;

namespace API.Application.CommandHandlers.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(UserRepository));
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(CreateUserCommand));

            var users = _userRepository.GetAll();

            if (users.Any(f => f.Email.Address == request.Email))
                throw new DomainException($"Email '{request.Email}' already exists.");

            if (users.Any(f => f.Crm.Number == request.Crm))
                throw new DomainException($"CRM '{request.Crm}' already exists.");

            var user = User.CreateUser(
                request.Name, 
                request.Email, 
                request.Crm, 
                request.PasswordHash, 
                request.Role, 
                request.Specialities);

            await _userRepository.Add(user);

            var entitySaved = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

            if (!entitySaved)
                return Guid.Empty;

            return user.Id;
        }
    }
}
