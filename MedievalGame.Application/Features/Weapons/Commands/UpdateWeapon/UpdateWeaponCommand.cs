using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;

namespace MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon
{
    public record UpdateWeaponCommand(
            Guid Id,
            string? Name,
            int? AttackPower,
            int? Durability,
            Guid? WeaponTypeId,
            Guid? RarityId
        ) : IRequest<WeaponDto>;
}
