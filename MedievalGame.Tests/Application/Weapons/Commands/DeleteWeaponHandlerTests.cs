using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Items.Commands.DeleteItem;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Weapons.Commands.DeleteWeapon;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Weapons.Commands
{
    public class DeleteWeaponHandlerTests
    {
        private readonly Mock<IWeaponRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMediator> _mockMediator;

        public DeleteWeaponHandlerTests()
        {
            _mockRepo = new Mock<IWeaponRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
        }

        #region Success Cases

        [Fact]
        public async Task Handle_ShouldReturnDeletedWeaponDto_WhenWeaponExists()
        {

            var weaponId = Guid.NewGuid();
            var weapon = new Weapon { Id = weaponId };

            _mockRepo.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync(weapon);
            _mockRepo.Setup(r => r.DeleteAsync(weapon.Id)).ReturnsAsync(weapon);
            _mockMapper.Setup(m => m.Map<WeaponDto>(weapon)).Returns(new WeaponDto { Id = weaponId, Name = weapon.Name });

            var handler = new DeleteWeaponHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            var result = await handler.Handle(new DeleteWeaponCommand(weaponId), CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(weaponId);

            _mockRepo.Verify(r => r.GetByIdAsync(weaponId), Times.Once);
            _mockRepo.Verify(r => r.DeleteAsync(weapon.Id), Times.Once);
            _mockMediator.Verify(m => m.Publish(It.IsAny<DeleteWeaponNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenWeaponDoesNotExist()
        {

            var weaponId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetByIdAsync(weaponId)).ReturnsAsync((Weapon?)null);

            var handler = new DeleteWeaponHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new DeleteWeaponCommand(weaponId), CancellationToken.None));

            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            _mockMediator.Verify(m => m.Publish(It.IsAny<DeleteWeaponNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            var command = new DeleteWeaponCommand(Guid.Empty);

            var handler = new DeleteWeaponHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(command, CancellationToken.None));

            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            _mockMediator.Verify(m => m.Publish(It.IsAny<DeleteWeaponNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        #endregion
    }
}
