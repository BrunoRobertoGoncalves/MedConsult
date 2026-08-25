using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
    {
        public UpdateUserEmailCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O id do usuário não pode ser vazio.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail não pode ser vazio.")
                .EmailAddress().WithMessage("O e-mail informado é inválido.");
        }
    }
}
