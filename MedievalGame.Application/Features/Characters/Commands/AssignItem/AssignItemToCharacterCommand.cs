using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Commands.AssignItem
{
    public record AssignItemToCharacterCommand(Guid CharacterId, Guid ItemId): IRequest<CharacterDto>
    {
    }
}
