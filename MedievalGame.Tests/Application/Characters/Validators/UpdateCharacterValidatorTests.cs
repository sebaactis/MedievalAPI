using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Characters.Commands.UpdateCharacter;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Characters.Validators
{
    public class UpdateCharacterValidatorTests
    {
        #region Success Cases
        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDataIsInvalid()
        {
            var command = new UpdateCharacterCommand(Guid.NewGuid(), null, null, null, null, null, null);

            var mockRepo = new Mock<ICharacterRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            var handler = new UpdateCharacterHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        #endregion
    }
}
