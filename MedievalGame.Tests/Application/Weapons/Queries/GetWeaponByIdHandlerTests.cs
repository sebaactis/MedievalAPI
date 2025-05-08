using AutoMapper;
using FluentAssertions;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Items.Queries.GetItem;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Application.Features.Weapons.Queries.GetWeaponById;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Weapons.Queries
{
    public class GetWeaponByIdHandlerTests
    {
        private readonly Mock<IWeaponRepository> mockRepo;
        private readonly Mock<IMapper> mockMapper;

        public GetWeaponByIdHandlerTests()
        {
            mockRepo = new Mock<IWeaponRepository>();
            mockMapper = new Mock<IMapper>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnWeaponDto_WhenWeaponExists()
        {
            var weaponId = Guid.NewGuid();

            var weapon = new Weapon
            {
                Id = weaponId,
                Name = "A",
                AttackPower = 100,
                Rarity = new Rarity { Id = Guid.NewGuid(), Name = "Common" },
                WeaponType = new WeaponType { Id = Guid.NewGuid(), Name = "Maze" }
            };

            var expectedDto = new WeaponDto
            {
                Id = weaponId,
                Name = weapon.Name,
                AttackPower = weapon.AttackPower,
                Rarity = weapon.Rarity.Name,
                WeaponType = weapon.WeaponType.Name
            };

            mockRepo.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);

            mockMapper.Setup(m => m.Map<WeaponDto>(weapon)).Returns(expectedDto);

            var handler = new GetWeaponByIdHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new GetWeaponByIdQuery(weaponId), CancellationToken.None);

            result.Should().BeEquivalentTo(expectedDto);
            result.Name.Should().Be(weapon.Name);
            mockRepo.Verify(r => r.GetByIdAsync(weaponId), Times.Once);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenWeaponDoesNotExist()
        {
            var weaponId = Guid.NewGuid();

            mockRepo.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync((Weapon)null);

            var handler = new GetWeaponByIdHandler(mockRepo.Object, mockMapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetWeaponByIdQuery(weaponId), CancellationToken.None));
        }

        #endregion
    }
}
