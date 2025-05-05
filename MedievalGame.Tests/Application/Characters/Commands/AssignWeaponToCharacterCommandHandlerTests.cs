
using AutoMapper;
using FluentAssertions;
using MedievalGame.Application.Features.Characters.Commands.AssignWeapon;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Characters.Commands
{
    public class AssignWeaponToCharacterCommandHandlerTests
    {
        private readonly Mock<ICharacterRepository> _characterRepository;
        private readonly Mock<IWeaponRepository> _weaponRepository;
        private readonly Mock<IMapper> _mapper;

        public AssignWeaponToCharacterCommandHandlerTests()
        {

            _characterRepository = new Mock<ICharacterRepository>();
            _weaponRepository = new Mock<IWeaponRepository>();
            _mapper = new Mock<IMapper>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_Should_AddWeaponToCharacter_WhenAssigningFirstWeapon()
        {

            var characterId = Guid.NewGuid();
            var weaponId = Guid.NewGuid();

            var character = new Character
            {
                Id = characterId,
                Weapons = new List<Weapon>()
            };

            var weapon = new Weapon { Id = weaponId };

            var expectedDto = new CharacterDto
            {
                Id = character.Id,
                Weapons = new List<WeaponDto> { new WeaponDto { Id = weaponId } }
            };

            _characterRepository.Setup(repo => repo.GetByIdAsync(characterId))
                              .ReturnsAsync(character);

            _weaponRepository.Setup(repo => repo.GetByIdAsync(weaponId))
                            .ReturnsAsync(weapon);

            _mapper.Setup(m => m.Map<CharacterDto>(character))
                   .Returns(expectedDto);

            var handler = new AssignWeaponToCharacterHandler(
                _characterRepository.Object,
                _weaponRepository.Object,
                _mapper.Object);

            var command = new AssignWeaponToCharacterCommand(characterId, weaponId);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Weapons.Should().HaveCount(1);
            result.Weapons.First().Id.Should().Be(weaponId);

            _characterRepository.Verify(repo => repo.UpdateAsync(character), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_AddSecondWeapon_WhenAssigningDifferentWeapon()
        {

            var characterId = Guid.NewGuid();
            var firstWeaponId = Guid.NewGuid();
            var secondWeaponId = Guid.NewGuid();

            var character = new Character
            {
                Id = characterId,
                Weapons = new List<Weapon> { new Weapon { Id = firstWeaponId } }
            };

            var newWeapon = new Weapon { Id = secondWeaponId };

            var expectedDto = new CharacterDto
            {
                Id = character.Id,
                Weapons = new List<WeaponDto> {
                    new WeaponDto { Id = firstWeaponId },
                    new WeaponDto { Id = secondWeaponId }
                }
            };

            _characterRepository.Setup(repo => repo.GetByIdAsync(characterId))
                              .ReturnsAsync(character);

            _weaponRepository.Setup(repo => repo.GetByIdAsync(secondWeaponId))
                            .ReturnsAsync(newWeapon);

            _mapper.Setup(m => m.Map<CharacterDto>(character))
                   .Returns(expectedDto);

            var handler = new AssignWeaponToCharacterHandler(
                _characterRepository.Object,
                _weaponRepository.Object,
                _mapper.Object);

            var command = new AssignWeaponToCharacterCommand(characterId, secondWeaponId);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Weapons.Should().HaveCount(2);
            result.Weapons.Should().Contain(w => w.Id == firstWeaponId);
            result.Weapons.Should().Contain(w => w.Id == secondWeaponId);

            _characterRepository.Verify(repo => repo.UpdateAsync(character), Times.Once);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_Should_NotAddDuplicate_WhenAssigningExistingWeapon()
        {

            var characterId = Guid.NewGuid();
            var weaponId = Guid.NewGuid();

            var character = new Character
            {
                Id = characterId,
                Weapons = new List<Weapon>
                {
                    new Weapon { Id = weaponId }
                }
            };

            var weapon = new Weapon { Id = weaponId };

            var expectedDto = new CharacterDto
            {
                Id = character.Id,
                Weapons = new List<WeaponDto> { new WeaponDto { Id = weaponId } }
            };

            _characterRepository.Setup(repo => repo.GetByIdAsync(characterId))
                              .ReturnsAsync(character);

            _weaponRepository.Setup(repo => repo.GetByIdAsync(weaponId))
                            .ReturnsAsync(weapon);

            _mapper.Setup(m => m.Map<CharacterDto>(character))
                   .Returns(expectedDto);

            var handler = new AssignWeaponToCharacterHandler(
                _characterRepository.Object,
                _weaponRepository.Object,
                _mapper.Object);

            var command = new AssignWeaponToCharacterCommand(characterId, weaponId);

            var result = await handler.Handle(command, CancellationToken.None);


            result.Should().NotBeNull();
            result.Weapons.Should().HaveCount(1); // Sigue teniendo solo un arma
            result.Weapons.First().Id.Should().Be(weaponId);

            _characterRepository.Verify(repo => repo.UpdateAsync(character), Times.Once);
        }

        #endregion
    }
}
