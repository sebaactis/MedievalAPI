using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;

namespace MedievalGame.Application.Features.Weapons.Queries.GetWeaponById
{
    public record GetWeaponByIdQuery(Guid Id) : IRequest<WeaponDto>
    {
    }
}
