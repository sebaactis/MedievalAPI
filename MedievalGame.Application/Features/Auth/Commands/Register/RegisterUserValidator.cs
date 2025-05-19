using FluentValidation;

namespace MedievalGame.Application.Features.Auth.Commands.Register
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(4).MaximumLength(20).WithMessage("Username must be between 4 and 20 characters.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(25).WithMessage("Password must be between 6 and 25 characters.");
        }
    }

}

