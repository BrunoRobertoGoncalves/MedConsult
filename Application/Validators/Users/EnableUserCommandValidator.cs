using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class EnableUserCommandValidator : AbstractValidator<EnableUserCommand>
    {
        public EnableUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O id do usuário não pode ser vazio.");
        }
    }
}
