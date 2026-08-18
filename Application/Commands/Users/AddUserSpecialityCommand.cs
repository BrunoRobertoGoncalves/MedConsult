using Domain.Enums;
using MediatR;

namespace Application.Commands.Users
{
    public class AddUserSpecialityCommand : IRequest<bool>
    {
        public AddUserSpecialityCommand(Guid id, SpecialityType speciality)
        {
            Id = id;
            Speciality = speciality;
        }

        public Guid Id { get; private set; }

        public SpecialityType Speciality { get; private set; }
    }
}
