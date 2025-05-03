using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;

namespace MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon
{
    public record UpdateWeaponNotification(WeaponDto weaponDto) : INotification
    {
    }
}
