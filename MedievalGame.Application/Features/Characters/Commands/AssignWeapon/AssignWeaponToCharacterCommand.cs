using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Commands.AssignWeapon
{
    public record AssignWeaponToCharacterCommand(Guid CharacterId, Guid WeaponId) : IRequest<CharacterDto>
    {
    }
}
