using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class UpdateUserNameCommandValidator : AbstractValidator<UpdateUserNameCommand>
    {
        public UpdateUserNameCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O id do usuário não pode ser vazio.");

            RuleFor(x => x.NewUserName)
                .NotEmpty().WithMessage("O nome não pode ser vazio.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais que 100 caracteres.");
        }
    }
}
