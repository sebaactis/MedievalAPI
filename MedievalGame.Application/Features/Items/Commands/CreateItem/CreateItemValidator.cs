using FluentValidation;

namespace MedievalGame.Application.Features.Items.Commands.CreateItem
{
    public class CreateItemValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(30)
                .WithMessage("Name must not exceed 30 characters");
            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("Value must be greater than 0");
            RuleFor(x => x.RarityId)
                .NotEmpty()
                .WithMessage("Rarity is required");
            RuleFor(x => x.ItemTypeId)
                .NotEmpty()
                .WithMessage("ItemType is required");
        }
    }
}
