using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class AddUserSpecialityCommandValidator : AbstractValidator<AddUserSpecialityCommand>
    {
        public AddUserSpecialityCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O id do usuário não pode ser vazio.");

            RuleFor(x => x.Speciality)
                .IsInEnum().WithMessage("Especialidade inválida.");
        }
    }
}
