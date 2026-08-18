using Domain.Enums;
using MediatR;

namespace Application.Commands.Users
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public CreateUserCommand(
            string name,
            string email,
            string crm,
            string passwordHash,
            UserRole role,
            IReadOnlyCollection<SpecialityType> specialities)
        {
            Name = name;
            Email = email;
            Crm = crm;
            PasswordHash = passwordHash;
            Role = role;
            Specialities = specialities;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Crm { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }
        public IReadOnlyCollection<SpecialityType> Specialities { get; private set; }
    }
}
