
using AutoMapper;
using FluentAssertions;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Items.Queries.GetItems;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Application.Features.Weapons.Queries.GetWeapons;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Weapons.Queries
{
    public class GetWeaponsHandlerTests
    {
        private readonly Mock<IWeaponRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetWeaponsHandlerTests()
        {
            _mockRepo = new Mock<IWeaponRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnListOfWeaponDto_WhenWeaponsExist()
        {
            var weapons = new List<Weapon>
        {
            new Weapon {
                Id = Guid.NewGuid(),
                Name = "A",
                AttackPower = 100,
                Rarity = new Rarity { Id = Guid.NewGuid(),Name = "Common" },
                WeaponType = new WeaponType{ Id = Guid.NewGuid(), Name = "Sword" }
            },

            new Weapon {
                Id = Guid.NewGuid(),
                Name = "B",
                AttackPower = 200,
                Rarity = new Rarity { Id = Guid.NewGuid(),Name = "Common" },
                WeaponType = new WeaponType{ Id = Guid.NewGuid(), Name = "Sword" }
            }
        };

            var expectedDtos = weapons.Select(c => new WeaponDto
            {
                Id = c.Id,
                Name = c.Name,
                AttackPower = c.AttackPower,
                Rarity = c.Rarity.Name,
                WeaponType = c.WeaponType.Name
            }).ToList();

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(weapons);

            _mockMapper.Setup(m => m.Map<List<WeaponDto>>(weapons)).Returns(expectedDtos);

            var handler = new GetWeaponsHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetWeaponsQuery(), CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDtos);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoWeaponsExist()
        {

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync((List<Weapon>)null);

            _mockMapper.Setup(m => m.Map<List<WeaponDto>>(It.IsAny<List<Weapon>>()))
              .Returns(new List<WeaponDto>());

            var handler = new GetWeaponsHandler(_mockRepo.Object, _mockMapper.Object);
            var result = await handler.Handle(new GetWeaponsQuery(), CancellationToken.None);

            result.Should().BeEmpty();
        }

        #endregion
    }
}
