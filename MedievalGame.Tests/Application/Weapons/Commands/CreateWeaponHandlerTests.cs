
using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Items.Commands.CreateItem;
using MedievalGame.Application.Features.Weapons.Commands.CreateWeapon;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Weapons.Commands
{
    public class CreateWeaponHandlerTests
    {

        private readonly Mock<IWeaponRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMediator> _mockMediator;

        public CreateWeaponHandlerTests()
        {
            _mockRepo = new Mock<IWeaponRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
        }


        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnCharacterDto_WhenCharacterIsCreated()
        {
            var command = new CreateWeaponCommand("Sword", 50, 100, Guid.NewGuid(), Guid.NewGuid());

            var weapon = new Weapon
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                AttackPower = command.AttackPower,
                Durability = command.Durability,
                WeaponType = new WeaponType { Id = command.WeaponTypeId, Name = "Sword" },
                Rarity = new Rarity { Id = command.RarityId, Name = "Common" }
            };

            var expectedDto = new WeaponDto
            {
                Id = weapon.Id,
                Name = weapon.Name,
                AttackPower = weapon.AttackPower,
                Rarity = weapon.Rarity.Name,
                WeaponType = weapon.WeaponType.Name
            };

            _mockMapper.Setup(m => m.Map<WeaponDto>(It.IsAny<Weapon>())).Returns(expectedDto);

            var handler = new CreateWeaponHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(weapon.Id);
            result.Name.Should().Be(command.Name);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Weapon>()), Times.Once);
            _mockMediator.Verify(p => p.Publish(It.IsAny<CreateWeaponNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowValidationsException_WhenRequiredAttributeIsMissing()
        {
            var command = new CreateWeaponCommand(null, 50, 100, Guid.NewGuid(), Guid.NewGuid());

            var handler = new CreateWeaponHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            await Assert.ThrowsAsync<ValidationsException>(() => handler.Handle(command, CancellationToken.None));

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Weapon>()), Times.Never);
            _mockMediator.Verify(p => p.Publish(It.IsAny<CreateWeaponNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion
    }
}
