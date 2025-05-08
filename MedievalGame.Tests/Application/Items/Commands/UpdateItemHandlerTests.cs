
using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Items.Commands.UpdateItem;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Items.Commands
{
    public class UpdateItemHandlerTests
    {
        private readonly Mock<IItemRepository> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IMediator> mockMediator;

        public UpdateItemHandlerTests()
        {
            mockRepo = new Mock<IItemRepository>();
            mockMapper = new Mock<IMapper>();
            mockMediator = new Mock<IMediator>();
        }


        #region Success Cases

        [Fact]
        public async Task Handle_ShouldReturnUpdatedItemDto_WhenItemrExistsAndDataIsValid()
        {
            var itemId = Guid.NewGuid();

            var command = new UpdateItemCommand(itemId, "Test", 100, Guid.NewGuid(), Guid.NewGuid());

            var item = new Item
            {
                Id = itemId,
                Name = "Pre Modif",
                Value = 100,
                RarityId = Guid.NewGuid(),
                ItemTypeId = Guid.NewGuid()
            };

            var updatedItem = new Item
            {
                Id = itemId,
                Name = command.Name!,
                Value = command.Value.Value!,
                RarityId = command.RarityId.Value!,
                ItemTypeId = command.ItemTypeId.Value!
            };

            var expectedDto = new ItemDto
            {
                Id = itemId,
                Name = command.Name!,
                Value = item.Value,
                Rarity = "Common",
                Type = "Weapon"
            };

            mockRepo.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Item>())).ReturnsAsync(updatedItem);

            mockMapper.Setup(m => m.Map<ItemDto>(updatedItem)).Returns(expectedDto);

            var handler = new UpdateItemHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeEquivalentTo(expectedDto);
            result.Name.Should().Be(command.Name);
            result.Value.Should().Be(command.Value);

            mockRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Once);

            mockMediator.Verify(r => r.Publish(It.IsAny<UpdateItemNotification>(), It.IsAny<CancellationToken>()), Times.Once);

        }
        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCharacterDoesNotExist()
        {
            var itemId = Guid.NewGuid();
            var command = new UpdateItemCommand(itemId, "Test", 100, Guid.NewGuid(), Guid.NewGuid());

            mockRepo.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync((Item)null);

            var handler = new UpdateItemHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        #endregion
    }
}
