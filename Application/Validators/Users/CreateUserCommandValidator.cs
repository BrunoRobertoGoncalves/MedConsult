using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome não pode ser vazio.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais que 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail não pode ser vazio.")
                .EmailAddress().WithMessage("O e-mail informado é inválido.");

            RuleFor(x => x.Crm)
                .NotEmpty().WithMessage("O CRM não pode ser vazio.")
                .Matches(@"^\d{4,6}\/[A-Z]{2}$").WithMessage("CRM inválido. Formato esperado: 123456/SP");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("A senha não pode ser vazia.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Função de usuário inválida.");

            RuleFor(x => x.Specialities)
                .NotEmpty().WithMessage("O médico deve ter pelo menos uma especialidade.");

            RuleForEach(x => x.Specialities)
                .IsInEnum().WithMessage("Especialidade inválida.");
        }
    }
}
