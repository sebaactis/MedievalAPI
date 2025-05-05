using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Characters.Commands.UpdateCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Characters.Commands
{
    public class UpdateCharacterHandlerTests
    {

        private readonly Mock<ICharacterRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMediator> _mockMediator;

        public UpdateCharacterHandlerTests()
        {
            _mockRepo = new Mock<ICharacterRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnUpdatedCharacterDto_WhenCharacterExistsAndDataIsValid()
        {
            var characterId = Guid.NewGuid();
            var command = new UpdateCharacterCommand(characterId, "Name Updated", 120, 150, 100, 2, Guid.NewGuid());

            var existingCharacter = new Character
            {
                Id = characterId,
                Name = "Old",
                Life = 100,
                Attack = 120,
                Defense = 90,
                Level = 1,
                CharacterClassId = Guid.NewGuid()
            };

            var updatedCharacter = new Character
            {
                Id = characterId,
                Name = command.Name!,
                Life = command.Life!.Value,
                Attack = command.Attack!.Value,
                Defense = command.Defense!.Value,
                Level = command.Level!.Value,
                CharacterClassId = command.CharacterClassId!.Value
            };

            var expectedDto = new CharacterDto
            {
                Id = characterId,
                Name = "Updated",
                Life = 120,
                Attack = 150,
                Defense = 100
            };

            _mockRepo.Setup(r => r.GetByIdAsync(characterId)).ReturnsAsync(existingCharacter);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Character>())).ReturnsAsync(updatedCharacter);

            _mockMapper.Setup(m => m.Map<CharacterDto>(updatedCharacter)).Returns(expectedDto);

            var handler = new UpdateCharacterHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(expectedDto);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Character>()), Times.Once);
            _mockMediator.Verify(m => m.Publish(It.IsAny<UpdateCharacterNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region Failure Cases
        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCharacterDoesNotExist()
        {

            var characterId = Guid.NewGuid();
            var command = new UpdateCharacterCommand(characterId, "Name", 100, 100, 100, 1, Guid.NewGuid());

            _mockRepo.Setup(r => r.GetByIdAsync(characterId)).ReturnsAsync((Character?)null);

            var handler = new UpdateCharacterHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
        #endregion

    }
}
