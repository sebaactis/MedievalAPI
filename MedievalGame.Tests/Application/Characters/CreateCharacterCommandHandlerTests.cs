using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Characters
{
    public class CreateCharacterCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnCharacterDto_WhenCharacterIsCreated()
        {
            var command = new CreateCharacterCommand("John Pepen", 100, 150, 200, 1, Guid.NewGuid());

            var mockRepo = new Mock<ICharacterRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockPublisher = new Mock<IMediator>();

            var character = new Character
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Life = command.Life,
                Attack = command.Attack,
                Defense = command.Defense,
                Level = command.Level,
                CharacterClassId = command.CharacterClassId
            };

            var expectedDto = new CharacterDto
            {
                Id = character.Id,
                Name = character.Name,
                Life = character.Life,
                Attack = character.Attack,
                Defense = character.Defense
            };

            mockMapper.Setup(m => m.Map<Character>(It.IsAny<CreateCharacterCommand>())).Returns(character);
            mockMapper.Setup(m => m.Map<CharacterDto>(It.IsAny<Character>())).Returns(expectedDto);

            var handler = new CreateCharacterHandler(mockRepo.Object, mockMapper.Object, mockPublisher.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(character.Id);
            result.Name.Should().Be("John Pepen");

            mockRepo.Verify(r => r.AddAsync(It.IsAny<Character>()), Times.Once);
            mockPublisher.Verify(p => p.Publish(It.IsAny<CreateCharacterNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
