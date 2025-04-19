using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Queries.GetWeapons
{
    public class GetWeaponsHandler(IWeaponRepository weaponRepository, IMapper mapper) : IRequestHandler<GetWeaponsQuery, List<WeaponDto>>
    {
        public async Task<List<WeaponDto>> Handle(GetWeaponsQuery request, CancellationToken cancellationToken)
        {
            var weapons = await weaponRepository.GetAllAsync() ?? new List<Domain.Entities.Weapon>();
            return mapper.Map<List<WeaponDto>>(weapons);
        }
    }
}
