using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;

namespace MedievalGame.Application.Features.Weapons.Commands.DeleteWeapon
{
    public record DeleteWeaponCommand(Guid Id) : IRequest<WeaponDto>
    {
    }
}
