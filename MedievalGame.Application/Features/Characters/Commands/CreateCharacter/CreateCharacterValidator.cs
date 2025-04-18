

using FluentValidation;

namespace MedievalGame.Application.Features.Characters.Commands.CreateCharacter
{
    public class CreateCharacterValidator : AbstractValidator<CreateCharacterCommand>
    {
        public CreateCharacterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithMessage("Name cannot be empty and must be less than 20 characters.");
            RuleFor(x => x.Life).NotEmpty().GreaterThan(0).WithMessage("Life must be greater than 0.");
            RuleFor(x => x.Attack).NotEmpty().GreaterThan(0).WithMessage("Attack must be greater than 0.");
            RuleFor(x => x.Defense).NotEmpty();
            RuleFor(x => x.Level).NotEmpty();
            RuleFor(x => x.CharacterClassId).NotEmpty();
        }
    }
}
