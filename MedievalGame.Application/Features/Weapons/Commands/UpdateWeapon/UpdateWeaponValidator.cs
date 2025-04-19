using FluentValidation;

namespace MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon
{
    public class UpdateWeaponValidator : AbstractValidator<UpdateWeaponCommand>
    {
        public UpdateWeaponValidator() {

            RuleFor(x => x.Name)
                .MaximumLength(20)
                .WithMessage("Name must be less than 20 characters.");
            RuleFor(x => x.AttackPower)
                .GreaterThan(0)
                .WithMessage("AttackPower must be greater than 0.");
            RuleFor(x => x.Durability)
                .GreaterThan(0)
                .WithMessage("Durability must be greater than 0.");
        }
    }
}
