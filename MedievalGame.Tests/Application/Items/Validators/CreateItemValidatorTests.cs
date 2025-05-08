using FluentAssertions;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Application.Features.Items.Commands.CreateItem;

namespace MedievalGame.Tests.Application.Items.Validators
{
    public class CreateItemValidatorTests
    {
        private readonly CreateItemValidator _validator = new();

        #region Success Cases
        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            var command = new CreateItemCommand("Sword", 100, Guid.NewGuid(), Guid.NewGuid());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
        #endregion

        #region Failure Cases

        [Fact]
        public void Should_Fail_When_Name_Is_Null()
        {
            var command = new CreateItemCommand(null, 100, Guid.NewGuid(), Guid.NewGuid());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Should_Fail_When_ItemTypeId_Is_Empty()
        {
            var command = new CreateItemCommand("Sword", 100, Guid.NewGuid(), Guid.Empty);

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "ItemTypeId");
        }

        #endregion
    }
}
