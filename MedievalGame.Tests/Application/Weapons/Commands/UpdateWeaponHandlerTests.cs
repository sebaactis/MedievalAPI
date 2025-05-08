using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Characters.Commands.UpdateCharacter;
using MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Weapons.Commands
{
    public class UpdateWeaponHandlerTests
    {
        private readonly Mock<IWeaponRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMediator> _mockMediator;

        public UpdateWeaponHandlerTests()
        {
            _mockRepo = new Mock<IWeaponRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnUpdatedWeaponDto_WhenWeaponExistsAndDataIsValid()
        {
            var weaponId = Guid.NewGuid();
            var command = new UpdateWeaponCommand(weaponId, "Updated Sword", 60, 90, Guid.NewGuid(), Guid.NewGuid());

            var existingWeapon = new Weapon
            {
                Id = weaponId,
                Name = "Old Sword",
                AttackPower = 50,
                Durability = 100,
                WeaponTypeId = Guid.NewGuid(),
                RarityId = Guid.NewGuid()
            };

            var updatedWeapon = new Weapon
            {
                Id = existingWeapon.Id,
                Name = command.Name,
                AttackPower = command.AttackPower.Value,
                Durability = command.Durability.Value,
                WeaponTypeId = command.WeaponTypeId.Value,
                RarityId = command.RarityId.Value
            };

            var expectedDto = new WeaponDto
            {
                Id = weaponId,
                Name = "Updated Sword",
                AttackPower = 60,
                Rarity = "Common",
                WeaponType = "Sword"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(existingWeapon);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Weapon>())).ReturnsAsync(updatedWeapon);

            _mockMapper.Setup(m => m.Map<WeaponDto>(updatedWeapon)).Returns(expectedDto);

            var handler = new UpdateWeaponHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(expectedDto);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Weapon>()), Times.Once);
            _mockMediator.Verify(m => m.Publish(It.IsAny<UpdateWeaponNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region Failure Cases
        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenWeaponDoesNotExist()
        {

            var weaponId = Guid.NewGuid();
            var command = new UpdateWeaponCommand(weaponId, "Updated Sword", 60, 90, Guid.NewGuid(), Guid.NewGuid());

            _mockRepo.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync((Weapon?)null);

            var handler = new UpdateWeaponHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        #endregion

    }
}
