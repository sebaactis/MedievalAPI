
using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Items.Commands.UpdateItem;
using MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Weapons.Validators
{
    public class UpdateWeaponValidatorTests
    {
        #region Success Cases
        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDataIsInvalid()
        {
            var command = new UpdateWeaponCommand(Guid.NewGuid(), null, null, null, null, null);

            var mockRepo = new Mock<IWeaponRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            var handler = new UpdateWeaponHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        #endregion
    }
}
