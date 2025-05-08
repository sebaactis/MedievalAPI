using FluentAssertions;
using MedievalGame.Application.Features.Items.Commands.CreateItem;
using MedievalGame.Application.Features.Weapons.Commands.CreateWeapon;

namespace MedievalGame.Tests.Application.Weapons.Validators
{
    public class CreateWeaponValidatorTests
    {
        private readonly CreateWeaponValidator _validator = new();

        #region Success Cases
        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            var command = new CreateWeaponCommand("Sword", 100, 200, Guid.NewGuid(), Guid.NewGuid());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
        #endregion

        #region Failure Cases

        [Fact]
        public void Should_Fail_When_Name_Is_Null()
        {
            var command = new CreateWeaponCommand(null, 100, 200, Guid.NewGuid(), Guid.NewGuid());

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Should_Fail_When_ItemTypeId_Is_Empty()
        {
            var command = new CreateWeaponCommand(null, 100, 200, Guid.NewGuid(), Guid.Empty);

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "RarityId");
        }

        #endregion
    }
}
