using AutoMapper;
using FluentAssertions;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Characters.Queries.GetCharacter;
using MedievalGame.Application.Features.Characters.Queries.GetCharacterById;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Characters.Queries
{
    public class GetCharacterByIdHandlerTests
    {

        private readonly Mock<ICharacterRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetCharacterByIdHandlerTests()
        {
            _mockRepo = new Mock<ICharacterRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnCharacterDto_WhenCharacterExists()
        {
            var characterId = Guid.NewGuid();

            var character = new Character
            {
                Id = characterId,
                Name = "John",
                Life = 100,
                Attack = 50,
                Defense = 30,
                Level = 1,
                CharacterClassId = Guid.NewGuid()
            };

            var expectedDto = new CharacterDto
            {
                Id = characterId,
                Name = "John",
                Life = 100,
                Attack = 50,
                Defense = 30
            };

            _mockRepo.Setup(r => r.GetByIdAsync(characterId)).ReturnsAsync(character);

            _mockMapper.Setup(m => m.Map<CharacterDto>(character)).Returns(expectedDto);

            var handler = new GetCharacterByIdHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetCharacterByIdQuery(characterId), CancellationToken.None);

            result.Should().BeEquivalentTo(expectedDto);
            _mockRepo.Verify(r => r.GetByIdAsync(characterId), Times.Once);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCharacterDoesNotExist()
        {
            var characterId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetByIdAsync(characterId)).ReturnsAsync((Character)null);

            var handler = new GetCharacterByIdHandler(_mockRepo.Object, _mockMapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetCharacterByIdQuery(characterId), CancellationToken.None));
        }

        #endregion
    }
}
