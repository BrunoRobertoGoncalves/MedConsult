using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class RemoveUserSpecialityCommandValidator : AbstractValidator<RemoveUserSpecialityCommand>
    {
        public RemoveUserSpecialityCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O id do usuário não pode ser vazio.");

            RuleFor(x => x.Speciality)
                .IsInEnum().WithMessage("Especialidade inválida.");
        }
    }
}
