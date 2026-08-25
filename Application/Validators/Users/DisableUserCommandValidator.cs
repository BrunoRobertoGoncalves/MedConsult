using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class DisableUserCommandValidator : AbstractValidator<DisableUserCommand>
    {
        public DisableUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O id do usuário não pode ser vazio.");
        }
    }
}
