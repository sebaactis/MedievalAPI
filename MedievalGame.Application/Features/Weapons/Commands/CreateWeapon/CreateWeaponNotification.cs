using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;

namespace MedievalGame.Application.Features.Weapons.Commands.CreateWeapon
{
    public record CreateWeaponNotification(WeaponDto weaponDto) : INotification
    {
    }
}
