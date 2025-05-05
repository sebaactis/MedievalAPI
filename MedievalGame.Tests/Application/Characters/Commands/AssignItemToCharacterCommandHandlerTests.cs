using AutoMapper;
using FluentAssertions;
using MedievalGame.Application.Features.Characters.Commands.AssignItem;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using Moq;

namespace MedievalGame.Tests.Application.Characters.Commands
{
    public class AssignItemToCharacterCommandHandlerTests
    {
        private readonly Mock<ICharacterRepository> _characterRepository;
        private readonly Mock<IItemRepository> _itemRepository;
        private readonly Mock<IMapper> _mapper;

        public AssignItemToCharacterCommandHandlerTests()
        {

            _characterRepository = new Mock<ICharacterRepository>();
            _itemRepository = new Mock<IItemRepository>();
            _mapper = new Mock<IMapper>();
        }

        #region Success Cases

        [Fact]
        public async Task Handle_Should_ReturnCharacterDtoWithOneQuantityOfItem_WhenItemIsAssignedFirstTime()
        {
            var characterId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var character = new Character
            {
                Id = characterId,
                CharacterItems = new List<CharacterItem>()
            };

            var item = new Item { Id = itemId };

            var expectedDto = new CharacterDto
            {
                Id = character.Id,
                Items = new List<CharacterItemDto> { new CharacterItemDto { ItemId = itemId, Quantity = 1 } }
            };

            _characterRepository.Setup(repo => repo.GetByIdAsync(characterId))
                               .ReturnsAsync(character);

            _itemRepository.Setup(repo => repo.GetByIdAsync(itemId))
                          .ReturnsAsync(item);

            _mapper.Setup(m => m.Map<CharacterDto>(character))
                   .Returns(expectedDto);

            var handler = new AssignItemToCharacterCommandHandler(
                _characterRepository.Object,
                _itemRepository.Object,
                _mapper.Object);

            var command = new AssignItemToCharacterCommand(characterId, itemId);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items.First().ItemId.Should().Be(itemId);
            result.Items.First().Quantity.Should().Be(1);
        }


        [Fact]
        public async Task Handle_Should_ReturnCharacterDtoWithTwoQuantityOfItem_WhenItemIsAssignedSecondTime()
        {
            var characterId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var character = new Character
            {
                Id = characterId,
                CharacterItems = new List<CharacterItem>()
            };

            var item = new Item { Id = itemId };


            _characterRepository.Setup(repo => repo.GetByIdAsync(characterId))
                               .ReturnsAsync(character);

            _itemRepository.Setup(repo => repo.GetByIdAsync(itemId))
                          .ReturnsAsync(item);

            _mapper.SetupSequence(m => m.Map<CharacterDto>(It.IsAny<Character>()))
                    .Returns(new CharacterDto
                    {
                        Id = character.Id,
                        Items = new List<CharacterItemDto> { new CharacterItemDto { ItemId = itemId, Quantity = 1 } }
                    })
                    .Returns(new CharacterDto
                    {
                        Id = character.Id,
                        Items = new List<CharacterItemDto> { new CharacterItemDto { ItemId = itemId, Quantity = 2 } }
                    });

            var handler = new AssignItemToCharacterCommandHandler(
                _characterRepository.Object,
                _itemRepository.Object,
                _mapper.Object);

            var command = new AssignItemToCharacterCommand(characterId, itemId);

            var firstAdd = await handler.Handle(command, CancellationToken.None);

            firstAdd.Should().NotBeNull();
            firstAdd.Items.Should().HaveCount(1);
            firstAdd.Items.First().ItemId.Should().Be(itemId);
            firstAdd.Items.First().Quantity.Should().Be(1);

            var secondAdd = await handler.Handle(command, CancellationToken.None);

            secondAdd.Should().NotBeNull();
            secondAdd.Items.Should().HaveCount(1);
            secondAdd.Items.First().ItemId.Should().Be(itemId);
            secondAdd.Items.First().Quantity.Should().Be(2);
        }

        #endregion

        #region Failure Cases

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenCharacterDoesNotExist()
        {
            var characterId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            _characterRepository.Setup(repo => repo.GetByIdAsync(characterId))
                               .ReturnsAsync((Character)null);

            _itemRepository.Setup(repo => repo.GetByIdAsync(itemId))
                          .ReturnsAsync(new Item { Id = itemId });

            var handler = new AssignItemToCharacterCommandHandler(
                _characterRepository.Object,
                _itemRepository.Object,
                _mapper.Object);

            var command = new AssignItemToCharacterCommand(characterId, itemId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            _characterRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Character>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenItemDoesNotExist()
        {

            var characterId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            _characterRepository.Setup(repo => repo.GetByIdAsync(characterId))
                               .ReturnsAsync(new Character { Id = characterId });

            _itemRepository.Setup(repo => repo.GetByIdAsync(itemId))
                          .ReturnsAsync((Item)null);

            var handler = new AssignItemToCharacterCommandHandler(
                _characterRepository.Object,
                _itemRepository.Object,
                _mapper.Object);

            var command = new AssignItemToCharacterCommand(characterId, itemId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            _characterRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Character>()), Times.Never);
        }

        #endregion
    }
}