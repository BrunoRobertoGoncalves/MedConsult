using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O id do usuário não pode ser vazio.");

            RuleFor(x => x.NewRole)
                .IsInEnum().WithMessage("Função de usuário inválida.");
        }
    }
}
