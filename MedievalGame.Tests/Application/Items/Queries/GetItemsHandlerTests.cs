using AutoMapper;
using FluentAssertions;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Characters.Queries.GetCharacters;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Items.Queries.GetItems;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Items.Queries
{
    public class GetItemsHandlerTests
    {
        private readonly Mock<IItemRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetItemsHandlerTests()
        {
            _mockRepo = new Mock<IItemRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnListOfItemDto_WhenItemsExist()
        {
            var items = new List<Item>
        {
            new Item {
                Id = Guid.NewGuid(),
                Name = "A",
                Value = 100,
                Rarity = new Rarity { Id = Guid.NewGuid(),Name = "Common" },
                ItemType = new ItemType{ Id = Guid.NewGuid(), Name = "Potion" }
            },

            new Item {
                Id = Guid.NewGuid(),
                Name = "B",
                Value = 100,
                Rarity = new Rarity { Id = Guid.NewGuid(),Name = "Common" },
                ItemType = new ItemType{ Id = Guid.NewGuid(), Name = "Potion" }
            },

        };

            var expectedDtos = items.Select(c => new ItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Value = c.Value,
                Rarity = c.Rarity.Name,
                Type = c.ItemType.Name

            }).ToList();

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

            _mockMapper.Setup(m => m.Map<List<ItemDto>>(items)).Returns(expectedDtos);

            var handler = new GetItemsHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetItemsQuery(), CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDtos);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoItemsExist()
        {

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync((List<Item>)null);

            _mockMapper.Setup(m => m.Map<List<ItemDto>>(It.IsAny<List<Item>>()))
              .Returns(new List<ItemDto>());

            var handler = new GetItemsHandler(_mockRepo.Object, _mockMapper.Object);
            var result = await handler.Handle(new GetItemsQuery(), CancellationToken.None);

            result.Should().BeEmpty();
        }

        #endregion
    }
}
