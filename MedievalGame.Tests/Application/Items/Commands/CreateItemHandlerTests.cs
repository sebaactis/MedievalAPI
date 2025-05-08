
using AutoMapper;
using FluentAssertions;
using MediatR;
using MedievalGame.Application.Features.Items.Commands.CreateItem;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Items.Commands
{
    public class CreateItemHandlerTests
    {
        private readonly Mock<IItemRepository> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IMediator> mockMediator;

        public CreateItemHandlerTests()
        {
            mockRepo = new Mock<IItemRepository>();
            mockMapper = new Mock<IMapper>();
            mockMediator = new Mock<IMediator>();
        }

        #region Success Cases

        [Fact]
        public async Task Handle_ShouldReturnWeaponDto_WhenWeaponIsCreate()
        {
            var command = new CreateItemCommand("Sword", 100, Guid.NewGuid(), Guid.NewGuid());

            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Value = command.Value,
                Rarity = new Rarity { Id = command.RarityId, Name = "Common" },
                ItemType = new ItemType { Id = command.ItemTypeId, Name = "Weapon" }
            };

            var expectedDto = new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Value = item.Value,
                Rarity = item.Rarity.Name,
                Type = item.ItemType.Name
            };

            mockMapper.Setup(m => m.Map<ItemDto>(It.IsAny<Item>())).Returns(expectedDto);

            var handler = new CreateItemHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeOfType<ItemDto>();
            result.Should().BeEquivalentTo(expectedDto);
            result.Name.Should().Be(command.Name);

            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Item>()), Times.Once);
            mockMediator.Verify(med => med.Publish(It.IsAny<CreateItemNotification>(), CancellationToken.None), Times.Once);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_ShouldThrowValidationsException_WhenRequiredAttributedIsMissing()
        {
            var command = new CreateItemCommand(null, 100, Guid.NewGuid(), Guid.NewGuid());

            var handler = new CreateItemHandler(mockRepo.Object, mockMapper.Object, mockMediator.Object);

            await Assert.ThrowsAsync<ValidationsException>(() => handler.Handle(command, CancellationToken.None));

            mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Item>()), Times.Never);
            mockMediator.Verify(med => med.Publish(It.IsAny<CreateItemNotification>(), CancellationToken.None), Times.Never);
        }

        #endregion
    }
}
