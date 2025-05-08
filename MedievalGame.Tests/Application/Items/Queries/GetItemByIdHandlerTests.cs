using AutoMapper;
using FluentAssertions;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Characters.Queries.GetCharacter;
using MedievalGame.Application.Features.Characters.Queries.GetCharacterById;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Items.Queries.GetItem;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Items.Queries
{
    public class GetItemByIdHandlerTests
    {
        private readonly Mock<IItemRepository> mockRepo;
        private readonly Mock<IMapper> mockMapper;

        public GetItemByIdHandlerTests()
        {
            mockRepo = new Mock<IItemRepository>();
            mockMapper = new Mock<IMapper>();
        }

        #region Success Cases
        [Fact]
        public async Task Handle_ShouldReturnItemDto_WhenItemExists()
        {
            var itemId = Guid.NewGuid();

            var item = new Item
            {
                Id = itemId,
                Name = "A",
                Value = 100,
                Rarity = new Rarity { Id = Guid.NewGuid(), Name = "Common" },
                ItemType = new ItemType { Id = Guid.NewGuid(), Name = "Potion" }
            };

            var expectedDto = new ItemDto
            {
                Id = itemId,
                Name = "A",
                Value = 100,
                Rarity = "Common",
                Type = "Potion"
            };

            mockRepo.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);

            mockMapper.Setup(m => m.Map<ItemDto>(item)).Returns(expectedDto);

            var handler = new GetItemByIdHandler(mockRepo.Object, mockMapper.Object);

            var result = await handler.Handle(new GetItemByIdQuery(itemId), CancellationToken.None);

            result.Should().BeEquivalentTo(expectedDto);
            result.Name.Should().Be(item.Name);
            mockRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenItemDoesNotExist()
        {
            var itemId = Guid.NewGuid();

            mockRepo.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync((Item)null);

            var handler = new GetItemByIdHandler(mockRepo.Object, mockMapper.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetItemByIdQuery(itemId), CancellationToken.None));
        }

        #endregion
    }
}
