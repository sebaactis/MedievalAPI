using FluentValidation;

namespace MedievalGame.Application.Features.Weapons.Commands.CreateWeapon
{
    public class CreateWeaponValidator : AbstractValidator<CreateWeaponCommand>
    {
        public CreateWeaponValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Name cannot be empty and must be less than 20 characters.");
            RuleFor(x => x.AttackPower)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("AttackPower must be greater than 0.");
            RuleFor(x => x.Durability)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Durability must be greater than 0.");
            RuleFor(x => x.RarityId)
                .NotEmpty()
                .WithMessage("Rarity cannot be empty");
            RuleFor(x => x.WeaponTypeId)
                .NotEmpty()
                .WithMessage("Rarity cannot be empty");
        }
    }
}
