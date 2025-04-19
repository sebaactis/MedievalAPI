using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;

namespace MedievalGame.Application.Features.Weapons.Queries.GetWeapons
{
    public record GetWeaponsQuery : IRequest<List<WeaponDto>>
    {
    }
}
