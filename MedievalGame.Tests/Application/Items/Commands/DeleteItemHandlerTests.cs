using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Characters.Commands.DeleteCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Items.Commands.DeleteItem;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Items.Commands
{
    public class DeleteItemHandlerTests
    {
        private readonly Mock<IItemRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMediator> _mockMediator;

        public DeleteItemHandlerTests()
        {
            _mockRepo = new Mock<IItemRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
        }

        #region Success Cases

        [Fact]
        public async Task Handle_ShouldReturnDeletedItemDto_WhenItemExists()
        {

            var itemId = Guid.NewGuid();
            var item = new Item { Id = itemId };

            _mockRepo.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);
            _mockRepo.Setup(r => r.DeleteAsync(item.Id)).ReturnsAsync(item);
            _mockMapper.Setup(m => m.Map<ItemDto>(item)).Returns(new ItemDto { Id = itemId, Name = item.Name });

            var handler = new DeleteItemHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            var result = await handler.Handle(new DeleteItemCommand(itemId), CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(itemId);

            _mockRepo.Verify(r => r.GetByIdAsync(itemId), Times.Once);
            _mockRepo.Verify(r => r.DeleteAsync(item.Id), Times.Once);
            _mockMediator.Verify(m => m.Publish(It.IsAny<DeleteItemNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenItemDoesNotExist()
        {

            var itemId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync((Item?)null);

            var handler = new DeleteItemHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new DeleteItemCommand(itemId), CancellationToken.None));

            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            _mockMediator.Verify(m => m.Publish(It.IsAny<DeleteItemNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            var command = new DeleteItemCommand(Guid.Empty);

            var handler = new DeleteItemHandler(_mockRepo.Object, _mockMapper.Object, _mockMediator.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(command, CancellationToken.None));

            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            _mockMediator.Verify(m => m.Publish(It.IsAny<DeleteItemNotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        #endregion
    }
}
