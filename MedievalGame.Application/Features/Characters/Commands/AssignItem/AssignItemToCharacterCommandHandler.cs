using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Commands.AssignItem
{
    public class AssignItemToCharacterCommandHandler(ICharacterRepository characterRepository, IItemRepository itemRepository, IMapper mapper) : IRequestHandler<AssignItemToCharacterCommand, CharacterDto>
    {

        public async Task<CharacterDto> Handle(AssignItemToCharacterCommand request, CancellationToken cancellationToken)
        {
            var character = await characterRepository.GetByIdAsync(request.CharacterId)
                ?? throw new NotFoundException("Character not found");

            var item = await itemRepository.GetByIdAsync(request.ItemId)
                ?? throw new NotFoundException("Item not found");

            character.AssignItem(item);

            await characterRepository.UpdateAsync(character);

            return mapper.Map<CharacterDto>(character);
        }
    }
}
