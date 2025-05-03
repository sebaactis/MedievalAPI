using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Characters.Commands.DeleteCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Characters.Commands
{
    public class DeleteCharacterHandlerTests
    {
        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnDeletedCharacterDto_WhenCharacterExists()
        {

            var characterId = Guid.NewGuid();
            var character = new Character { Id = characterId };

            var mockRepo = new Mock<ICharacterRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            mockRepo.Setup(r => r.GetByIdAsync(characterId)).ReturnsAsync(character);
            mockRepo.Setup(r => r.DeleteAsync(character.Id)).ReturnsAsync(character);
            mockMapper.Setup(m => m.Map<CharacterDto>(character)).Returns(new CharacterDto { Id = characterId, Name = character.Name });

            var handler = new DeleteCharacterHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            var result = await handler.Handle(new DeleteCharacterCommand(characterId), CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(characterId);

            mockRepo.Verify(r => r.GetByIdAsync(characterId), Times.Once);
            mockRepo.Verify(r => r.DeleteAsync(character.Id), Times.Once);
            mockMediator.Verify(m => m.Publish(It.IsAny<DeleteCharacterNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCharacterDoesNotExist()
        {

            var characterId = Guid.NewGuid();

            var mockRepo = new Mock<ICharacterRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            mockRepo.Setup(r => r.GetByIdAsync(characterId)).ReturnsAsync((Character?)null);

            var handler = new DeleteCharacterHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new DeleteCharacterCommand(characterId), CancellationToken.None));

            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            mockMediator.Verify(m => m.Publish(It.IsAny<DeleteCharacterNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            var command = new DeleteCharacterCommand(Guid.Empty);

            var mockRepo = new Mock<ICharacterRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediator = new Mock<IMediator>();

            var handler = new DeleteCharacterHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(command, CancellationToken.None));

            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            mockMediator.Verify(m => m.Publish(It.IsAny<DeleteCharacterNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        #endregion
    }
}
