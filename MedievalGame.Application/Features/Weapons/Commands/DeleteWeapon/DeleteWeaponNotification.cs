using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;

namespace MedievalGame.Application.Features.Weapons.Commands.DeleteWeapon
{
    public record DeleteWeaponNotification(WeaponDto weaponDto) : INotification
    {
    }
}
