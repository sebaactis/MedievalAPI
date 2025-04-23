
using FluentValidation;

namespace MedievalGame.Application.Features.Items.Commands.UpdateItem
{
    public class UpdateItemValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemValidator() {

            RuleFor(x => x.Name)
                .MaximumLength(30)
                .WithMessage("Name must not exceed 30 characters");
            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("Value must be greater than 0");
        }
    }
}
