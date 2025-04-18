using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;

namespace MedievalGame.Application.Features.Weapons.Commands.CreateWeapon
{
    public record CreateWeaponCommand(
            string Name,
            int AttackPower,
            int Durability,
            Guid WeaponTypeId,
            Guid RarityId
        ) : IRequest<WeaponDto>;
}
