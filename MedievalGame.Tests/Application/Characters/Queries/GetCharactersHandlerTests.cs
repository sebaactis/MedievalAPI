using AutoMapper;
using FluentAssertions;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Characters.Queries.GetCharacters;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Characters.Queries
{
    public class GetCharactersHandlerTests
    {
        private readonly Mock<ICharacterRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetCharactersHandlerTests()
        {
            _mockRepo = new Mock<ICharacterRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnListOfCharacterDto_WhenCharactersExist()
        {
            var characters = new List<Character>
        {
            new Character { Id = Guid.NewGuid(), Name = "A", Life = 100, Attack = 50, Defense = 30, Level = 1, CharacterClassId = Guid.NewGuid() },
            new Character { Id = Guid.NewGuid(), Name = "B", Life = 120, Attack = 60, Defense = 40, Level = 2, CharacterClassId = Guid.NewGuid() }
        };

            var expectedDtos = characters.Select(c => new CharacterDto
            {
                Id = c.Id,
                Name = c.Name,
                Life = c.Life,
                Attack = c.Attack,
                Defense = c.Defense
            }).ToList();

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(characters);

            _mockMapper.Setup(m => m.Map<List<CharacterDto>>(characters)).Returns(expectedDtos);

            var handler = new GetCharactersHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetCharacterQuery(), CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDtos);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCharactersExist()
        {

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync((List<Character>)null);

            _mockMapper.Setup(m => m.Map<List<CharacterDto>>(It.IsAny<List<Character>>())).Returns(new List<CharacterDto>());

            var handler = new GetCharactersHandler(_mockRepo.Object, _mockMapper.Object);
            var result = await handler.Handle(new GetCharacterQuery(), CancellationToken.None);

            result.Should().BeEmpty();
        }

        #endregion
    }

}
