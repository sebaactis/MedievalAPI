using FluentAssertions;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;

namespace MedievalGame.Tests.Application.Characters.Validators
{
    public class CreateCharacterValidatorTests
    {
        private readonly CreateCharacterValidator _validator = new();

        #region Success Cases
        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            var command = new CreateCharacterCommand("Valid", 100, 100, 100, 1, Guid.NewGuid());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
        #endregion

        #region Failure Cases

        [Fact]
        public void Should_Fail_When_Name_Is_Null()
        {
            var command = new CreateCharacterCommand(null, 200, 300, 150, 5, Guid.NewGuid());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Should_Fail_When_CharacterClassId_Is_Empty()
        {
            var command = new CreateCharacterCommand("Test", 100, 100, 100, 1, Guid.Empty);

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CharacterClassId");
        }

        #endregion
    }
}
