using FluentValidation;

namespace MedievalGame.Application.Features.Characters.Commands.UpdateCharacter
{
    public class UpdateCharacterValidator : AbstractValidator<UpdateCharacterCommand>
    {
        public UpdateCharacterValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(x => x.Name).MaximumLength(20).WithMessage("Name cannot be empty and must be less than 20 characters.");
            RuleFor(x => x.Life).GreaterThan(0).WithMessage("Life must be greater than 0.");
            RuleFor(x => x.Attack).GreaterThan(0).WithMessage("Attack must be greater than 0.");
        }
    }
}
